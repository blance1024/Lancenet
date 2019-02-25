using Microsoft.AspNetCore.Builder;

namespace Lanetnet.AspnetCore.APILog
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseResponseTime(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseTimeMiddleware>();
        }
    }
}
