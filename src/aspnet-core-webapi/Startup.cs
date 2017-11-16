using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using de.playground.aspnet.core.utils.swagger.DocumentFilters;

namespace de.playground.aspnet.core.webapi
{
    public class Startup
    {
        #region Private Fields

        private readonly IConfiguration configuration;

        #endregion

        #region Constructor

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        #endregion

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddApiVersioning();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1", new Info { Title = "API V1", Version = "v1" });
                // setupAction.SwaggerDoc("v2", new Info { Title = "API V2", Version = "v2" });

                setupAction.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var versions = apiDesc.ControllerAttributes().OfType<ApiVersionAttribute>().SelectMany(attribute => attribute.Versions);
                    return versions.Any(version => $"v{version.ToString()}" == docName);
                });

                setupAction.OperationFilter<RemoveVersionParameters>();
                setupAction.DocumentFilter<SetVersionInPaths>();
            });

            services.AddTransient(typeof(ICustomerModule), typeof(CustomerModule));
            services.AddTransient(typeof(IProductModule), typeof(ProductModule));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                //setupAction.SwaggerEndpoint("/swagger/v2/swagger.json", "V2 Docs");
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
            });
        }
    }
}
