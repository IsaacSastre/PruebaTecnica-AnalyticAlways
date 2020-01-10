using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PTAnalitic.Core.Interfaces.Repositories;
using PTAnalitic.Core.UnitOfWork;
using System;
using System.Linq;

namespace PTAnalitic.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly PTDbContext _dbContext;
        readonly ILogger _logger;
        readonly IServiceProvider _serviceProvider;

        Lazy<IProductHistoryRepository> _productHistoryRepository;

        public UnitOfWork(PTDbContext dbContext, ILogger<UnitOfWork> logger, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public IProductHistoryRepository ProductHistoryRepository
        {
            get
            {
                if (_productHistoryRepository == null)
                    _productHistoryRepository = new Lazy<IProductHistoryRepository>(() => (IProductHistoryRepository)_serviceProvider.GetService(typeof(IProductHistoryRepository)));

                return _productHistoryRepository.Value;
            }
        }

        public int Save()
        {
            try
            {
                return _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error saving dbContext. Concurrency exception");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error saving dbContext. Update exception");
            }            
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving dbContext");
            }

            return -1;
        }

        public void DetachAllEntities()
        {
            foreach(var entry in _dbContext.ChangeTracker.Entries().ToArray())
            {
                if (entry.Entity != null)
                    entry.State = EntityState.Detached;
            }
        }

        public void Dispose()
        {
            try
            {
                _dbContext.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disposing dbContext");
            }
        }
    }
}