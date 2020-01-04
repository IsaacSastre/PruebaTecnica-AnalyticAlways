using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTAnalitic.Core.Extensions;
using System.IO;

namespace PTAnalitic
{
    public class Startup
    {
        IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServices(Configuration);
        }
    }
}