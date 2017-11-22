using System;
using de.playground.aspnet.core.contracts.dataaccesses;
using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.dataaccesses.inmemory;
using de.playground.aspnet.core.modules;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace de.playground.net.core.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServiceCollection(services);

            var serviceProvider = services.BuildServiceProvider();
            ConfigureServiceProvider(serviceProvider);

            var mainDialog = serviceProvider.GetService<MainDialog>();
            mainDialog.ShowAsync().Wait();
        }

        private static void ConfigureServiceCollection(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceCollection.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            serviceCollection.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

            serviceCollection.AddTransient<ICustomerModule, CustomerModule>();
            serviceCollection.AddTransient<IProductModule, ProductModule>();
            serviceCollection.AddTransient<ICustomerDataAccess, CustomerInMemoryDataAccess>();
            serviceCollection.AddTransient<IProductDataAccess, ProductInMemoryDataAccess>();
            serviceCollection.AddTransient<MainDialog>();
            serviceCollection.AddTransient<CustomerDialog>();
        }

        private static void ConfigureServiceProvider(IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            loggerFactory.AddConsole(LogLevel.Trace);
            loggerFactory.AddDebug();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            loggerFactory.ConfigureNLog("nlog.config");
        }
    }
}
