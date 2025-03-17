using System.Text;
using System.Text.Json;

namespace workout_tracker.api.Services;

public interface IOpenAIService
{
    Task<string> GetOpenAIResponseAsync(string prompt);   
}

public class OpenAIService : IOpenAIService
{
    private static readonly HttpClient client = new();
    private readonly string openAIUrl;
    private readonly string apiKey;
    private readonly string systemMessage = "You are a workout coach.";

    public OpenAIService(string openAIUrl, string apiKey)
    {
        this.openAIUrl = openAIUrl;
        this.apiKey = apiKey;
    }
    
    public async Task<string> GetOpenAIResponseAsync(string prompt)
    {
        // Clear any existing Authorization headers
        if (client.DefaultRequestHeaders.Contains("Authorization"))
        {
            client.DefaultRequestHeaders.Remove("Authorization");
        }

        // Set up the request
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var requestBody = new
        {
            model = "gpt-4", // Specify the model you want to use
            messages = new[]
            {
                new { role = "system", content = systemMessage },
                new { role = "user", content = prompt }
            }
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Send the request
        var response = await client.PostAsync(openAIUrl, content);
        var responseString = await response.Content.ReadAsStringAsync();

        // Parse the response
        dynamic jsonResponse = JsonSerializer.Deserialize<dynamic>(responseString);
        string message = jsonResponse.choices[0].message.content;

        return message;
    }
}