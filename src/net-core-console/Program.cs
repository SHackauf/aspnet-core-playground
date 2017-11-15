using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.modules;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace de.playground.net.core.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            var mainDialog = serviceProvider.GetService<MainDialog>();
            mainDialog.ShowAsync().Wait();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(new LoggerFactory()
                .AddConsole(LogLevel.Trace)
                .AddDebug());
            serviceCollection.AddLogging();

            serviceCollection.AddTransient<ICustomerModule, CustomerModule>();
            serviceCollection.AddTransient<IProductModule, ProductModule>();
            serviceCollection.AddTransient<MainDialog>();
            serviceCollection.AddTransient<CustomerDialog>();
        }
    }
}
