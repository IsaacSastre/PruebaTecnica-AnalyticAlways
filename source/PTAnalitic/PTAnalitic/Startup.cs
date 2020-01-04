using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTAnalitic.Core.Extensions;
using Serilog;
using System;
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

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File(
                    $"{Directory.GetCurrentDirectory()}/Logs/PTAnalitic-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    fileSizeLimitBytes: 500_000,
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1))
            .CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServices(Configuration);
        }
    }
}