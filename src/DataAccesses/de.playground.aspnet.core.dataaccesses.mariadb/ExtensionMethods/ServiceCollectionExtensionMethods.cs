using de.playground.aspnet.core.contracts.dataaccesses;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace de.playground.aspnet.core.dataaccesses.mariadb.ExtensionMethods
{
    // TODO: Find a better way to register dependency injection from moduls.
    public static class ServiceCollectionExtensionMethods
    {
        public static void ConfigureServicesMariaDbDataAccess(this IServiceCollection services, IConfiguration Configuration, bool initializeWithServiceScope)
        {
            services.AddDbContext<MariaDbContext>(options => options.UseMySql(Configuration.GetConnectionString("MariaDbConnection")));

            services.AddTransient(typeof(ICustomerDataAccess), typeof(CustomerMariaDbDataAccess));
            services.AddTransient(typeof(IProductDataAccess), typeof(ProductMariaDbDataAccess));

            if (initializeWithServiceScope)
            {
                services.AddTransient<IDataAccessInitialize, MariaDbServiceScopeInitialize>();
            }
            else
            {
                services.AddTransient<IDataAccessInitialize, MariaDbInitialize>();
            }
            services.AddTransient<IDataAccessInitialize, MariaDbInitialize>();
        }
    }
}
