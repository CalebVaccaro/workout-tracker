using Microsoft.Extensions.Configuration;

namespace workout_tracker;

public static class AppConfig
{
    public static string GetOpenAPIKey()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json");

        IConfigurationRoot configuration = builder.Build();

        string apiKey = configuration["apiKey"];
        return apiKey;
    }
}