namespace LL.Core.Model.DataTransferObjects;

public class AgentModel
{
    public string GeminiAPI { get; set; } = string.Empty;
    
    public string LLamaModeLocation { get; set; } = string.Empty;

    public AgentModel(string geminiAPI, string llamaModeLocation)
    {
        GeminiAPI = geminiAPI;
        LLamaModeLocation = llamaModeLocation;
    }
}