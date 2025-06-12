using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using DotNetEnv;
using CloudinaryDotNet;

namespace BiomechanicNetwork.Utilities
{
    internal static class SecretsManager
    {
        private static readonly IConfiguration _configuration;

        static SecretsManager()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<AppConfig>(); // Используем наш нестатический класс

            _configuration = builder.Build();
        }

        public static string GetConnectionString()
        {
            return _configuration.GetConnectionString("PostgreSQL");
        }

        public static string GetCloudinaryCloudName()
        {
            var cloudName = _configuration["Cloudinary:CLOUDINARY_CLOUD_NAME"];
            Console.WriteLine($"Cloudinary Cloud Name: {cloudName}");
            return cloudName;
        }

        public static string GetCloudinaryApiKey()
        {
            var apiKey = _configuration["Cloudinary:CLOUDINARY_API_KEY"];
            Console.WriteLine($"Cloudinary API Key: {apiKey}");
            return apiKey;
        }

        public static string GetCloudinaryApiSecret()
        {
            var apiSecret = _configuration["Cloudinary:CLOUDINARY_API_SECRET"];
            Console.WriteLine($"Cloudinary API Secret: {apiSecret}");
            return apiSecret;
        }
    }
    public class AppConfig { }
}
