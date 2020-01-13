using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PTAnalitic.Core.Interfaces.Repositories;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PTAnalitic.Infrastructure.Repositories
{
    public class ProductHistoryRepository : IProductHistoryRepository
    {
        private PTDbContext _dbContext;
        private ILogger _logger;
        IConfiguration _configuration;

        public ProductHistoryRepository(PTDbContext dbContext, ILogger<ProductHistoryRepository> logger, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
        }

        public bool AddRange(DataTable dt)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("PTContext");

                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_InsertProductHistories", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter param = cmd.Parameters.AddWithValue("@productHistories", dt);
                param.SqlDbType = SqlDbType.Structured;

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error adding entities with stored procedure");

                return false;
            }
        }

        public bool DeleteImport()
        {
            try
            {
                var result =  _dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo].[ProductHistory]");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error deleting previous import");

                return false;
            }
        }
    }
}