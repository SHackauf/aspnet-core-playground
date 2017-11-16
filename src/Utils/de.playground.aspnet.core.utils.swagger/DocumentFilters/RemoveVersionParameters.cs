using System.Linq;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace de.playground.aspnet.core.utils.swagger.DocumentFilters
{
    public class RemoveVersionParameters : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // Remove version parameter from all Operations
            var versionParameter = operation.Parameters.Single(parameter => parameter.Name == "version");
            operation.Parameters.Remove(versionParameter);
        }
    }
}
