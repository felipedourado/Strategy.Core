using System.IO;
using Microsoft.Extensions.Configuration;


namespace Strategy.Core.Configurations
{
    public static class AppSettings
    {
        public static T Get<T>(string key) =>
            GetConfiguration().GetSection(key).Get<T>();

        private static IConfiguration GetConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            return configuration;
        }

        public static string GetParameter(string value)
        {
            return GetConfiguration().GetSection(value).Value;
        }
    }
}