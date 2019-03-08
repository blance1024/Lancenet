using Lanetnet.AspnetCore.APILog;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Builder
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseResponseTime(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseTimeMiddleware>();
        }
    }
}
