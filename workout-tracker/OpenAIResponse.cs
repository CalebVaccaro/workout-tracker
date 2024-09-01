using Newtonsoft.Json;
using System.Text;

namespace workout_tracker;

public class OpenAIResponse
{
    private static readonly HttpClient client = new HttpClient();
    
    public static async Task<string> GetOpenAIResponseAsync(string apiKey, string prompt)
    {
        // Set up the request
        var requestUri = "https://api.openai.com/v1/chat/completions";
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var requestBody = new
        {
            model = "gpt-4", // Specify the model you want to use
            messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = prompt }
            }
        };

        var jsonContent = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Send the request
        var response = await client.PostAsync(requestUri, content);
        var responseString = await response.Content.ReadAsStringAsync();

        // Parse the response
        dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
        string message = jsonResponse.choices[0].message.content;

        return message;
    }
}