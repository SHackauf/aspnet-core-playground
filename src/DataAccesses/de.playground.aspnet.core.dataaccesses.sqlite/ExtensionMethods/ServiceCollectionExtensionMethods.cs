using de.playground.aspnet.core.contracts.dataaccesses;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace de.playground.aspnet.core.dataaccesses.sqlite.ExtensionMethods
{
    public static class ServiceCollectionExtensionMethods
    {
        public static void ConfigureServicesSqLiteDbDataAccess(this IServiceCollection services, IConfiguration Configuration, bool initializeWithServiceScope)
        {
            services.AddDbContext<SqLiteDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("SqLiteDbConnection")));

            services.AddTransient(typeof(ICustomerDataAccess), typeof(CustomerSqLiteDbDataAccess));
            services.AddTransient(typeof(IProductDataAccess), typeof(ProductSqLiteDbDataAccess));

            if (initializeWithServiceScope)
            {
                services.AddTransient<IDataAccessInitialize, SqLiteDbServiceScopeInitialize>();
            }
            else
            {
                services.AddTransient<IDataAccessInitialize, SqLiteDbInitialize>();
            }
        }
    }
}
