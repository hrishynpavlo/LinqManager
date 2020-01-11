using Microsoft.Extensions.DependencyInjection;

namespace LinqManager.WebExtensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddLinqManager(this IServiceCollection services)
        {
            services.AddSingleton<LinqManager>();
            return services;
        }
    }
}