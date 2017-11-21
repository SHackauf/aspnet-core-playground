using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Owin;
using Microsoft.Extensions.Logging;

namespace de.playground.aspnet.core.servers.middlewares
{
    public class ExampleMiddleware
    {
        #region Private Fields

        private readonly RequestDelegate next;
        private readonly ILogger logger;

        #endregion

        #region Constructor

        public ExampleMiddleware(RequestDelegate next, ILogger<ExampleMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        #endregion

        #region Public Methods

        public Task Invoke(HttpContext context)
        {
            var owinEnvironment = new OwinEnvironment(context);
            var owinFeatures = new OwinFeatureCollection(owinEnvironment);

            var requestId = owinFeatures.Environment.ContainsKey("owin.RequestId") ? owinFeatures.Environment["owin.RequestId"] : string.Empty;
            var requestMethod = owinFeatures.Environment.ContainsKey("owin.RequestMethod") ? owinFeatures.Environment["owin.RequestMethod"] : string.Empty;
            var requestPath = owinFeatures.Environment.ContainsKey("owin.RequestPath") ? owinFeatures.Environment["owin.RequestPath"] : string.Empty;
            
            this.logger.LogInformation("[RequestId: {0}][RequestMethod: {1}][RequestPath: {2}]", requestId, requestMethod, requestPath);

            return this.next(context);
        }

        #endregion
    }
}
