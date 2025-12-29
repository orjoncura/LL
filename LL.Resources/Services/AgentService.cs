using System.Diagnostics;
using System.Text;
using Google.GenAI;
using Google.GenAI.Types;
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
        var client = new Client(apiKey: agentModel.GeminiAPI);
        var response = await client.Models.GenerateContentAsync(model: "gemini-2.5-flash", contents: input);
        
        Candidate? candidate = response.Candidates?.FirstOrDefault();
        string output = candidate?.Content?.Parts?.Select(x => x.Text).Aggregate((x, y) => x + " " + y) ?? string.Empty;

        // Read the response
        return output;
    }
    private async Task<string> RunLocalOllama(string input)
    { 
        IChatClient chatClient =  new OllamaChatClient(new Uri(agentModel.LLamaModeLocation), "qwen2.5:14b");
        
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

