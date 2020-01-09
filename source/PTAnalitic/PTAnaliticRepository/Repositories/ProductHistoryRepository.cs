using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PTAnalitic.Core.Interfaces.Repositories;
using PTAnalitic.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTAnalitic.Infrastructure.Repositories
{
    public class ProductHistoryRepository : IProductHistoryRepository
    {
        private PTDbContext _dbContext;
        private ILogger _logger;

        public ProductHistoryRepository(PTDbContext dbContext, ILogger<ProductHistoryRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> AddRange(IList<ProductHistory> productHistories)
        {
            try
            {
                await _dbContext.AddRangeAsync(productHistories);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error adding entities");

                return false;
            }
        }

        public async Task<bool> DeleteImport()
        {
            try
            {
                await _dbContext.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE [ProductHistory]");

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