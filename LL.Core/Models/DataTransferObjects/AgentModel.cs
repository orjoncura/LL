namespace LL.Core.Model.DataTransferObjects;

public class AgentModel
{
    public string GeminiAPI { get; set; } = string.Empty;
    
    public string LLamaModeLocation { get; set; } = string.Empty;
    
    public string LocalModelName { get; set; } = "qwen2.5:14b";
    
    public string OfflineModelName { get; set; } = "qwen2.5:1.5b";
    
    public string Mode { get; set; } = "online";

    public AgentModel(
        string geminiAPI,
        string llamaModeLocation,
        string localModelName = "qwen2.5:14b",
        string offlineModelName = "qwen2.5:1.5b",
        string mode = "online")
    {
        GeminiAPI = geminiAPI;
        LLamaModeLocation = llamaModeLocation;
        LocalModelName = string.IsNullOrWhiteSpace(localModelName) ? "qwen2.5:14b" : localModelName;
        OfflineModelName = string.IsNullOrWhiteSpace(offlineModelName) ? "qwen2.5:1.5b" : offlineModelName;
        Mode = string.IsNullOrWhiteSpace(mode) ? "online" : mode.Trim().ToLowerInvariant();
    }
}