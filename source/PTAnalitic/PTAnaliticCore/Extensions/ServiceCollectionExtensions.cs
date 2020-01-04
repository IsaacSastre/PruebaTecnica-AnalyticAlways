using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTAnalitic.Infrastructure.Extensions;

namespace PTAnalitic.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepositories(configuration);
        }
    }
}