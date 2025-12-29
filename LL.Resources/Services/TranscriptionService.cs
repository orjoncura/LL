using System.Text;
using LL.Core.Constants;
using LL.Core.Interfaces.Extensions;
using LL.Core.Helpers;
using Microsoft.Extensions.Configuration;

namespace LL.Resources.Services;

public class TranscriptionService(IConfiguration configuration) : ITranscriptionService
{  
    public async Task<string> TranscribeFromUrl(string videoUrl)
    {
        var api = configuration[Secrets.TranscriptionApi] ?? string.Empty;
        
        var client = new HttpClient();
        var requestBody = new { url = videoUrl };
        var json = JsonHelper.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync(api, content);
        
        if (response.IsSuccessStatusCode) {
            // This will be the plain transcription string
            return await response.Content.ReadAsStringAsync();
        } 
        
        // This will be the JSON error message from FastAPI
        string errorJson = await response.Content.ReadAsStringAsync();
        throw new Exception(errorJson);
    }
}