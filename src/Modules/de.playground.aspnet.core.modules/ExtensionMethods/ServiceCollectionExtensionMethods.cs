using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace de.playground.aspnet.core.dataaccesses.mariadb.ExtensionMethods
{
    // TODO: Find a better way to register dependency injection from moduls.
    public static class ServiceCollectionExtensionMethods
    {
        public static void ConfigureServicesModules(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient(typeof(ICustomerModule), typeof(CustomerModule));
            services.AddTransient(typeof(IProductModule), typeof(ProductModule));
        }
    }
}
