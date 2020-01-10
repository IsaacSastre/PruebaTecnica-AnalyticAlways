using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PTAnalitic.Core.Interfaces.Repositories;
using PTAnalitic.Core.Interfaces.Services;
using PTAnalitic.Core.Model;
using PTAnalitic.Core.UnitOfWork;
using System;
using System.Collections.Generic;
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

        private const int NUMBER_ROWS_LOOP = 1000000;

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
                    int initRowCount, endRowCount = 0;

                    do
                    {
                        initRowCount = endRowCount;

                        endRowCount = await ReadFile(tempFileName, productHistoryRepo, endRowCount);

                    } while (initRowCount != endRowCount);

                    Console.WriteLine("Import finished");

                    File.Delete(tempFileName);

                    Console.WriteLine("File deleted");
                }

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

                        int i = 0;
                        do
                        {
                            records = csvReader.GetRecords<ProductHistory>().Skip(rowCount).Take(NUMBER_ROWS_LOOP).ToList();

                            if (!await WriteData(records, productHistoryRepo))
                                return 0;

                            _unitOfWork.Save();

                            rowCount += records.Count;

                            _unitOfWork.DetachAllEntities();

                            i++;
                            Console.WriteLine($"{rowCount} entries saved ");
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

        private async Task<bool> WriteData(IList<ProductHistory> productHistories, IProductHistoryRepository productHistoryRepo)
        {
            try
            {
                return await productHistoryRepo.AddRange(productHistories);
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