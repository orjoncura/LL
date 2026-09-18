using Google.GenAI;
using Google.GenAI.Types;
using LL.Core.Interfaces.Extensions;
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

        var candidate = response.Candidates?.FirstOrDefault();
        return candidate?.Content?.Parts?
            .Select(x => x.Text)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Aggregate(string.Empty, (current, next) =>
                string.IsNullOrEmpty(current) ? next! : current + " " + next)
            ?? string.Empty;
    }

    private async Task<string> RunOllama(string input, string modelName)
    {
        IChatClient chatClient = new OllamaChatClient(GetOllamaUri(), modelName);
        var chatHistory = new List<ChatMessage>
        {
            new(ChatRole.User, input)
        };

        var response = "";
        await foreach (var item in chatClient.GetStreamingResponseAsync(chatHistory))
            response += item.Text;

        return response;
    }

    public async Task<string> Run(string input)
    {
        if (!string.IsNullOrWhiteSpace(agentModel.GeminiAPI))
            return await RunGeminiApi(input);

        var modelName = string.Equals(agentModel.Mode, "offline", StringComparison.OrdinalIgnoreCase)
            ? agentModel.OfflineModelName
            : agentModel.LocalModelName;

        return await RunOllama(input, modelName);
    }
}
