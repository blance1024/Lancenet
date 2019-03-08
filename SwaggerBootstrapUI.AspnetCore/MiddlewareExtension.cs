using SwaggerBootstrapUI.AspnetCore;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseSwaggerBootstrapUI(
            this IApplicationBuilder app,
            Action<SwaggerBootstrapUIOptions> setupAction = null)
        {
            if (setupAction == null)
            {
                // Don't pass options so it can be configured/injected via DI container instead
                app.UseMiddleware<SwaggerBootstrapUIMiddleware>();
            }
            else
            {
                // Configure an options instance here and pass directly to the middleware
                var options = new SwaggerBootstrapUIOptions();
                setupAction.Invoke(options);

                app.UseMiddleware<SwaggerBootstrapUIMiddleware>(options);
            }

            return app;
        }
    }
}
