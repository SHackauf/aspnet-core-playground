using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.modules;

using Microsoft.Extensions.DependencyInjection;

namespace de.playground.net.core.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddTransient<ICustomerModule, CustomerModule>();
            services.AddTransient<IProductModule, ProductModule>();
            services.AddTransient<MainDialog>();
            services.AddTransient<CustomerDialog>();

            var serviceProvider = services.BuildServiceProvider();

            var mainDialog = serviceProvider.GetService<MainDialog>();
            mainDialog.ShowAsync().Wait();
        }
    }
}
