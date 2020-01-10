using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTAnalitic.Core.Interfaces.Services;
using PTAnalitic.Core.Services;

namespace PTAnalitic.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IProductHistoryService), typeof(ProductHistoryService));
        }
    }
}