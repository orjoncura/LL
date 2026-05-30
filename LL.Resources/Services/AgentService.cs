using Google.GenAI;
using Google.GenAI.Types;
using LL.Core.Interfaces.Extensions;
using LL.Extensions.Models;
using LL.Core.Model.DataTransferObjects;
using Microsoft.Extensions.AI;

namespace LL.Resources.Services;

public class AgentService(AgentModel agentModel) : IAgentService
{
    private Uri GetOllamaUri()
    {
        if (Uri.TryCreate(agentModel.LLamaModeLocation, UriKind.Absolute, out var configuredUri))
            return configuredUri;

        return new Uri("http://localhost:11434");
    }

    private async Task<string> RunGeminiApi(string input)
    {
        var client = new Client(apiKey: agentModel.GeminiAPI);
        var response = await client.Models.GenerateContentAsync(model: "gemini-2.5-flash", contents: input);
        
        Candidate? candidate = response.Candidates?.FirstOrDefault();
        string output = candidate?.Content?.Parts?.Select(x => x.Text).Aggregate((x, y) => x + " " + y) ?? string.Empty;

        // Read the response
        return output;
    }
    private async Task<string> RunOllama(string input, string modelName)
    { 
        IChatClient chatClient =  new OllamaChatClient(GetOllamaUri(), modelName);
        
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

    private Task<string> RunLocalOllama(string input) => RunOllama(input, agentModel.LocalModelName);
    
    private Task<string> RunOfflineOllama(string input) => RunOllama(input, agentModel.OfflineModelName);

    public async Task<string> Run(string input)
    {
        return await RunGeminiApi(input);
    } 
}

