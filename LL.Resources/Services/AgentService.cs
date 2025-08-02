using System.Diagnostics;
using System.Text;
using LL.Core.Helpers;
using LL.Core.Interfaces.Extensions;
using LL.Extensions.Models;
using LL.Core.Model.DataTransferObjects;
using Microsoft.Extensions.AI;

namespace LL.Resources.Services;

public class AgentService(AgentModel agentModel) : IAgentService
{
    private async Task<string> RunGeminiApi(string input)
    {
        // Replace with your actual Google API key
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={agentModel.GeminiAPI}";
        
        var payload = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new { text = input } // Pass the text input here
                    }
                }
            },
            safety_settings = new {
                category = "HARM_CATEGORY_SEXUALLY_EXPLICIT",
                threshold = "BLOCK_LOW_AND_ABOVE"
            },
            generation_config = new
            {
                temperature = 1,
                topP = 0.95,
                topK = 64,
                maxOutputTokens = 8192,
                response_mime_type = "application/json"
            }
        };
        
        // Serialize the payload to JSON
        string jsonPayload = JsonHelper.SerializeObject(payload);

        // Create an HttpClient instance
        using (HttpClient client = new HttpClient())
        {
            // Create the HTTP content with the JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        
            // Send the POST request
            HttpResponseMessage response = await client.PostAsync(url, content);

            // Check if the status code is NOT in the 2xx range
            if (!response.IsSuccessStatusCode) 
            {
                // Attempt to read the error content from the response
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Reason: {response.ReasonPhrase} - Status code: {(int)response.StatusCode} - Details: {errorContent}");
            }
            
            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            Candidate? candidate = JsonHelper.Extract<JsonData>(response.Content.ReadAsStringAsync().Result)?.Candidates?.FirstOrDefault();
            string output = candidate?.Content?.Parts?.Select(x => x.Text).Aggregate((x, y) => x + " " + y) ?? string.Empty;

            // Read the response
            return output;
        }
    }
    private async Task<string> RunLocalOllama(string input)
    { 
        IChatClient chatClient =  new OllamaChatClient(new Uri(agentModel.LLamaModeLocation), "deepseek-r1:14b");
        
        List<ChatMessage> chatHistory = new();

        while (true)
        {
            chatHistory.Add(new ChatMessage(ChatRole.User, input));

            var response = "";
            
            await foreach (var item in chatClient.GetStreamingResponseAsync(chatHistory))
            {
                response += item.Text;
            }

            return response;
        }
    }

    public async Task<string> Run(string input)
    {
        if (string.IsNullOrWhiteSpace(agentModel.GeminiAPI))
            return await RunLocalOllama(input);
        
        return await RunGeminiApi(input);
    } 
}

