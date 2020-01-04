using Microsoft.Extensions.DependencyInjection;
using PTAnalitic.Core.Extensions;
using System;

namespace PTAnalitic
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            Startup startup = new Startup();
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
        }
    }
}
