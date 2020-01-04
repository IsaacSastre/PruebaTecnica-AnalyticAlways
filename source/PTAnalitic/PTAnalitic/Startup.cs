using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTAnalitic.Core.Extensions;

namespace PTAnalitic
{
    public class Startup
    {
        IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServices(Configuration);
        }
    }
}