using LLama.Common;
using System.Text;
using LL.Core.Helpers;
using LL.Core.Interfaces.Extensions;
using LLama;
using LL.Extensions.Models;
using LL.Core.Model.DataTransferObjects;
using Microsoft.Extensions.AI;

namespace LL.Extensions.Services;

public class AgentService(AgentModel agentModel) : IAgentService
{
    private async Task<string> RunGeminiAPI(string input)
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

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            Candidate? candidate = JsonHelper.Extract<JsonData>(response.Content.ReadAsStringAsync().Result)?.Candidates?.FirstOrDefault();
            string output = candidate?.Content?.Parts?.Select(x => x.Text).Aggregate((x, y) => x + " " + y) ?? string.Empty;

            // Read the response
            return output;
        }
    }
    
    private async Task<string> RunLocalLlama(string input)
    {
        var parameters = new ModelParams(agentModel.LLamaModeLocation)
        {
            GpuLayerCount = 16 // How many layers to offload to GPU. Please adjust it according to your GPU memory.
        };

        using var model = LLamaWeights.LoadFromFile(parameters);
        using var context = model.CreateContext(parameters);
        var executor = new InteractiveExecutor(context);

        ChatSession session = new(executor);

        InferenceParams inferenceParams = new InferenceParams()
        {
            AntiPrompts = new List<string> { "User:" } // Stop generation once antiprompts appear.
        };

        string output = "";

        await foreach (var text in session.ChatAsync(new ChatHistory.Message(AuthorRole.User, input), inferenceParams))
            output += text;

        return output;
    }

    private async Task<string> RunLocalOllama(string input)
    {
        IChatClient chatClient =  new OllamaChatClient(new Uri("http://localhost:11434/"), "deepseek-r1:14b");
        
        List<ChatMessage> chatHistory = new();

        while (true)
        {
            chatHistory.Add(new ChatMessage(ChatRole.User, input));

            var response = "";
            
            await foreach (var item in chatClient.CompleteStreamingAsync(chatHistory))
            {
                response += item.Text;
            }

            return response;
        }

    }
    public async Task<string> Run(string input) => await RunLocalOllama(input);
}

