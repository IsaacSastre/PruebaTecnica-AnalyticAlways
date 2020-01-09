using Microsoft.Extensions.Logging;
using PTAnalitic.Core.Interfaces.Repositories;
using PTAnalitic.Core.UnitOfWork;
using System;
using System.Threading.Tasks;

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

        public IProductHistoryRepository ProductHistoryRepository => throw new NotImplementedException();

        public async Task<int> Save()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving dbContext");
            }

            return -1;
        }

        public IProductHistoryRepository LanguageRepository
        {
            get
            {
                if (_productHistoryRepository == null)
                    _productHistoryRepository = new Lazy<IProductHistoryRepository>(() => (IProductHistoryRepository)_serviceProvider.GetService(typeof(IProductHistoryRepository)));

                return _productHistoryRepository.Value;
            }
        }
    }
}