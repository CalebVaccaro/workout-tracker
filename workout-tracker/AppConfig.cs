using Microsoft.Extensions.Configuration;

namespace workout_tracker;

public static class AppConfig
{
    private static IConfigurationRoot? GetConfigurationRoot()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json");

        IConfigurationRoot configuration = builder.Build();
        return configuration;
    }
    
    public static string GetOpenAIAPIKey() => GetConfigurationRoot()["apiKey"];

    public static string GetOpenAIUrl() => GetConfigurationRoot()["openai-url"];
}