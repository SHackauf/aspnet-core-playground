using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using de.playground.aspnet.core.utils.swagger.DocumentFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace de.playground.aspnet.core.utils.swagger.ExtensionMethods
{
    public static class ServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddSwaggerGenMultiVersions(this IServiceCollection serviceCollection, Func<IEnumerable<Info>> getVersions, Func<ApiVersion, string> formatVersion)
        {
            return serviceCollection.AddSwaggerGen(setupAction =>
            {
                getVersions().ToList().ForEach(version => setupAction.SwaggerDoc(version.Version, version));

                setupAction.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var apiVersions = apiDesc.ControllerAttributes().OfType<ApiVersionAttribute>().SelectMany(attribute => attribute.Versions);
                    return apiVersions.Any(version => formatVersion(version) == docName);
                });

                setupAction.OperationFilter<RemoveVersionParameters>();
                setupAction.DocumentFilter<SetVersionInPaths>();
            });
        }
    }
}
