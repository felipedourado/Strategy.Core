using Microsoft.Extensions.DependencyInjection;
using Strategy.Core.Repositories.Interfaces;
using Strategy.Core.Repositories;
using Strategy.Core.Services.Interfaces;
using Strategy.Core.Services;

namespace Strategy.Core.Configurations
{
    public static class DependencyInjectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IProducts, ProductAService>();
            services.AddSingleton<IProducts, ProductBService>();
            services.AddSingleton<IStrategyContext, StrategyContext>();
        }
    }
}
