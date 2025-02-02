using System.Text;
using LL.Core.Interfaces.Extensions;
using Newtonsoft.Json;

namespace LL.Extensions.Services;

public class TranslationService: ITranslationService
{
    public async Task<string> TranslateText(string text, int fromId, int toId)
    {
        // Base URL of the local LibreTranslate server
        string baseUrl = "http://localhost:5000"; // Adjust if running on a different host/port
        string endpoint = "/translate";          // API endpoint

        // Translation request payload
        var payload = new
        {
            q = text.ToLower(), // Text to translate
            source = GetLanguageCode(fromId), // Source language
            target = GetLanguageCode(toId), // Target language
            format = "text" // Text format
        };
        
        // Serialise the payload to JSON
        string jsonPayload = JsonConvert.SerializeObject(payload);
        
        using (HttpClient client = new HttpClient())
        {
            // Set headers (if necessary, depending on your LibreTranslate setup)
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            // Send POST request
            HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(baseUrl + endpoint, content);

            // Ensure success status
            response.EnsureSuccessStatusCode();

            // Read response
            string responseBody = await response.Content.ReadAsStringAsync();

            // Parse JSON response
            dynamic result = JsonConvert.DeserializeObject(responseBody);
            
            return  result.translatedText;
        }
    }

    private string GetLanguageCode(int id)
    {
        string languageCode = "en";
        
        if(id == 2)
            languageCode = "es";

        return languageCode;
    }
}