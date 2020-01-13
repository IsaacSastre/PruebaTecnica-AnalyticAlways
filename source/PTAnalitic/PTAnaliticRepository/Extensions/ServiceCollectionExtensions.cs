using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using PTAnalitic.Core.Interfaces.Repositories;
using PTAnalitic.Core.UnitOfWork;
using PTAnalitic.Infrastructure.Repositories;

namespace PTAnalitic.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static readonly LoggerFactory LoggerFactory =
            new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });

        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork));

            services.AddScoped(typeof(IProductHistoryRepository), typeof(ProductHistoryRepository));

            var connectionString = ConfigurationExtensions.GetConnectionString(configuration, "PTContext");
            services.AddDbContext<PTDbContext>(options =>
                options
                    .UseSqlServer(connectionString));
                    //.UseLoggerFactory(LoggerFactory)
                    //.EnableSensitiveDataLogging());

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<PTDbContext>();
        }    
    }
}