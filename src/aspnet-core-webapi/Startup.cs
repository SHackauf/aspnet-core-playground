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
using de.playground.aspnet.core.utils.swagger.ExtensionMethods;

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

        #region Private Properties

        public IEnumerable<(Info SwaggerInfo, string SwaggerEndpointUrl, string SwaggerEndpointDescription)> ApiVersions { get; } = new[]
        {
            (new Info { Title = "API V1", Version = "v1" }, "/swagger/v1/swagger.json", "V1 Docs")
        };

        #endregion

        #region Public Methods

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddApiVersioning();

            services.AddSwaggerGenMultiVersions( () => this.ApiVersions.Select(versions => versions.SwaggerInfo), apiVersion => $"v{apiVersion.ToString()}");

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
                this.ApiVersions.ToList().ForEach(version => setupAction.SwaggerEndpoint(version.SwaggerEndpointUrl, version.SwaggerEndpointDescription));
            });
        }

        #endregion
    }
}
