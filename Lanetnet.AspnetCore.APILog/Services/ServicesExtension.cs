using Lanetnet.AspnetCore.APILog.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lanetnet.AspnetCore.APILog.Services
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddApiLogService(this IServiceCollection services)
        {
            services.TryAddSingleton<IApiLogService, ApiLogServiceDefault>();
            return services;
        }

        public static IServiceCollection AddApiLogService<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddSingleton<TService, TImplementation>().AddApiLogService();
            return services;
        }
    }
}
