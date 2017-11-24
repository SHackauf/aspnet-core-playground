using de.playground.aspnet.core.contracts.dataaccesses;
using de.playground.aspnet.core.dataaccesses.inmemory;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace de.playground.aspnet.core.dataaccesses.mariadb.ExtensionMethods
{
    // TODO: Find a better way to register dependency injection from moduls.
    public static class ServiceCollectionExtensionMethods
    {
        public static void ConfigureServicesInMemoryDataAccess(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient(typeof(ICustomerDataAccess), typeof(CustomerInMemoryDataAccess));
            services.AddTransient(typeof(IProductDataAccess), typeof(ProductInMemoryDataAccess));

            services.AddTransient<IDataAccessInitialize, InMemoryInitialize>();
        }
    }
}
