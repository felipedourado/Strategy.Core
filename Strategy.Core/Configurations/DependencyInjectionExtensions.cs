using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Strategy.Core.Domain.Interfaces.Repositories;
using Strategy.Core.Domain.Interfaces.Services;
using Strategy.Core.Domain.Settings;
using Strategy.Core.Infra.Repositories;
using Strategy.Core.Services;

namespace Strategy.Core.Configurations
{
    public static class DependencyInjectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProducts, DigitalAccountService>();
            services.AddScoped<IProducts, PhysicalAccountService>();
            services.AddScoped<IStrategy, Context>();

            services.AddScoped(typeof(IMongoGenericRepository<>), typeof(MongoGenericRepository<>));
        }

        public static void RegisterSettings(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MongoSettings>(option =>
            {
                option.ConnectionString = config.GetSection("MongoDb:ConnectionString").Value;
                option.DatabaseName = config.GetSection("MongoDb:DatabaseName").Value;
            });

            services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<MongoSettings>>().Value);
        }
    }
}
