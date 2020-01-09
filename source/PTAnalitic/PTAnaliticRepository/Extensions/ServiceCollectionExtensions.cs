using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTAnalitic.Core.UnitOfWork;

namespace PTAnalitic.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork));

            var connectionString = ConfigurationExtensions.GetConnectionString(configuration, "PTContext");
            services.AddDbContext<PTDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<PTDbContext>();
        }    
    }
}