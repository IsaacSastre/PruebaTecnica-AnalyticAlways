using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PTAnalitic.Core.Interfaces.Repositories;
using PTAnalitic.Core.Interfaces.Services;
using PTAnalitic.Core.Model;
using PTAnalitic.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PTAnalitic.Core.Services
{
    public class ProductHistoryService : IProductHistoryService
    {
        IUnitOfWork _unitOfWork;
        ILogger<ProductHistoryService> _logger;
        IConfiguration _configuration;

        private const int NUMBER_ROWS_LOOP = 5000000;

        public ProductHistoryService(IUnitOfWork unitOfWork, ILogger<ProductHistoryService> logger, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> ImportDataFromAzure()
        {
            try
            {
                using (_unitOfWork)
                {
                    var productHistoryRepo = _unitOfWork.ProductHistoryRepository;

                    await RemovePreviousImport(productHistoryRepo);

                    await ReadAndImportDataFromAzure(productHistoryRepo);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing data");

                return false;
            }
        }

        private async Task<bool> ReadAndImportDataFromAzure(IProductHistoryRepository productHistoryRepo)
        {
            try
            {
                var resourcePath = _configuration["ResourcePath"];

                var client = new WebClient();
                var tempFileName = "OutputTemp.csv";

                Console.WriteLine($"Downloading file to {tempFileName}");

                client.DownloadFile(resourcePath, tempFileName);

                Console.WriteLine($"File downloaded{Environment.NewLine}Starting data processing");

                if (File.Exists(tempFileName))
                {
                    Stopwatch t0 = new Stopwatch();
                    t0.Start();
                    int initRowCount, endRowCount = 0;

                    do
                    {
                        initRowCount = endRowCount;

                        endRowCount = await ReadFile(tempFileName, productHistoryRepo, endRowCount);

                        if (initRowCount == endRowCount)
                            endRowCount = await ReadFile(tempFileName, productHistoryRepo, endRowCount);

                    } while (initRowCount != endRowCount);
                    t0.Stop();

                    Console.WriteLine("Import finished");
                    Console.WriteLine($"Elapsed time: {t0.ElapsedMilliseconds / 1000} seg");

                    File.Delete(tempFileName);

                    Console.WriteLine("File deleted");
                }
                else
                    Console.WriteLine("File not found. Error downloading file");                

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file");

                return false;
            }
        }

        private async Task<int> ReadFile(string tempFileName, IProductHistoryRepository productHistoryRepo, int rowCount)
        {
            try
            {
                using (var sourceStream = File.OpenRead(tempFileName))
                {
                    using (var reader = new StreamReader(sourceStream))
                    {
                        var csvConfiguration = new CsvHelper.Configuration.Configuration
                        {
                            MissingFieldFound = null,
                            HeaderValidated = null
                        };

                        var csvReader = new CsvReader(reader, csvConfiguration);
                        List<ProductHistory> records = new List<ProductHistory>();

                        IEnumerable<ProductHistory> recordsEnumerable = csvReader.GetRecords<ProductHistory>();

                        int i = 0;
                        do
                        {
                            var recordsToWrite = recordsEnumerable.Skip(rowCount).Take(NUMBER_ROWS_LOOP);

                            DataTable dt = ConvertToDataTable(recordsToWrite);

                            if (!await WriteData(dt, productHistoryRepo))
                                return 0;

                            _unitOfWork.Save();

                            rowCount += dt.Rows.Count;
                            if (dt.Rows.Count > 0)
                                Console.WriteLine($"{rowCount} entries saved ");

                            _unitOfWork.DetachAllEntities();

                            i++;
                        } while (records.Count == NUMBER_ROWS_LOOP);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading data");

                return rowCount;
            }

            return rowCount;
        }

        private DataTable ConvertToDataTable(IEnumerable<ProductHistory> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(ProductHistory));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (ProductHistory item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            return table;
        }

        private async Task<bool> WriteData(DataTable dt, IProductHistoryRepository productHistoryRepo)
        {
            try
            {
                return productHistoryRepo.AddRange(dt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing data");

                return false;
            }
        }

        private async Task<bool> RemovePreviousImport(IProductHistoryRepository productHistoryRepo)
        {
            try
            {
                productHistoryRepo.DeleteImport();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing previous data");

                return false;
            }
        }
    }
}