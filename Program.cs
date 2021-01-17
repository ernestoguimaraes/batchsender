using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventHubBenchmark
{
    class Program
    {
        private static IConfigurationRoot configuration;


        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            //EventHub Sender
            var hs = new HubSender();
            hs.SendInBatch(configuration);


            //ServiceBus Sender
            var bs = new BusSender();
            bs.SendInBatch(configuration);

            Console.ReadKey();

            await host.RunAsync();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            
            serviceCollection.AddLogging();

            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appSettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);
            
        }
        
        static IHostBuilder CreateHostBuilder(string[] args) =>
              Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();

                    

                    configuration
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);                    
                });
     
    }
}
