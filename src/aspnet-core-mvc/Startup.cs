using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using de.playground.aspnet.core.contracts.dataaccesses;
using de.playground.aspnet.core.contracts.modules;
using de.playground.aspnet.core.dataaccesses.inmemory;
using de.playground.aspnet.core.dataaccesses.mariadb;
using de.playground.aspnet.core.dataaccesses.mariadb.ExtensionMethods;
using de.playground.aspnet.core.dataaccesses.sqlite.ExtensionMethods;
using de.playground.aspnet.core.modules;
using de.playground.aspnet.core.servers.middlewares.ExtensionMethods;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.mvc
{
    public class Startup
    {
        #region Constructor

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Private Properties

        public IConfiguration Configuration { get; }

        #endregion

        #region Public Methods

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAutoMapper();
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.ConfigureServicesModules(this.Configuration);
            //services.ConfigureServicesMariaDbDataAccess(this.Configuration, true);
            services.ConfigureServicesSqLiteDbDataAccess(this.Configuration, true);

            // TODO: Per option setzen
            //services.ConfigureServicesInMemoryDataAccess(this.Configuration, true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
            }

            app.UseExample();
            app.UseStaticFiles();
            app.UseResponseCompression();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.ApplicationServices.GetService<IDataAccessInitialize>().Initialize();
        }

        #endregion
    }
}
