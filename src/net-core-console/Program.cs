using System;
using System.IO;
using AutoMapper;
using de.playground.aspnet.core.contracts.dataaccesses;
using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.dataaccesses.inmemory;
using de.playground.aspnet.core.dataaccesses.mariadb.ExtensionMethods;
using de.playground.aspnet.core.dataaccesses.sqlite.ExtensionMethods;
using de.playground.aspnet.core.modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace de.playground.net.core.console
{
    class Program
    {
        #region Private Fields

        private readonly IConfiguration configuration;
        private readonly ServiceProvider serviceProvider;

        #endregion

        #region Static Main Method

        static void Main(string[] args)
        {
            var program = new Program();
            program.Start();
        }

        #endregion

        #region Constructor

        public Program()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            this.configuration = builder.Build();

            var services = new ServiceCollection();
            ConfigureServiceCollection(services);

            var serviceProvider = services.BuildServiceProvider();
            ConfigureServiceProvider(serviceProvider);
            this.serviceProvider = serviceProvider;
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            var mainDialog = serviceProvider.GetService<MainDialog>();
            mainDialog.ShowAsync().Wait();
        }

        #endregion

        #region Private Methods

        private void ConfigureServiceCollection(IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper();

            serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceCollection.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            serviceCollection.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

            serviceCollection.ConfigureServicesModules(this.configuration);
            serviceCollection.ConfigureServicesSqLiteDbDataAccess(this.configuration, false);
            //serviceCollection.ConfigureServicesMariaDbDataAccess(this.configuration);

            // TODO: Per option setzen
            //serviceCollection.ConfigureServicesInMemoryDataAccess(this.configuration);

            serviceCollection.AddTransient<MainDialog>();
            serviceCollection.AddTransient<CustomerDialog>();
            serviceCollection.AddTransient<XmlDialog>();
        }

        private void ConfigureServiceProvider(IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            loggerFactory.AddConsole(LogLevel.Trace);
            loggerFactory.AddDebug();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            loggerFactory.ConfigureNLog("nlog.config");
        }

        #endregion
    }
}
