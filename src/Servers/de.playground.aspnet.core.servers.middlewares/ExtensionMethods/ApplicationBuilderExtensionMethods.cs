using Microsoft.AspNetCore.Builder;

namespace de.playground.aspnet.core.servers.middlewares.ExtensionMethods
{
    public static class ApplicationBuilderExtensionMethods
    {
        public static IApplicationBuilder UseExample(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExampleMiddleware>();
    }
}
