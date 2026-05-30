namespace LL.Core.Model.DataTransferObjects;

public class AgentModel
{
    public string GeminiAPI { get; set; } = string.Empty;
    
    public string LLamaModeLocation { get; set; } = string.Empty;
    
    public string LocalModelName { get; set; } = string.Empty;
    
    public string OfflineModelName { get; set; } = string.Empty;
    
    public string Mode { get; set; } = string.Empty;

    public AgentModel(
        string geminiAPI,
        string llamaModeLocation,
        string localModelName,
        string offlineModelName ,
        string mode)
    {
        GeminiAPI = geminiAPI;
        LLamaModeLocation = llamaModeLocation;
        LocalModelName = localModelName;
        OfflineModelName = offlineModelName;
        Mode =  mode.Trim().ToLowerInvariant();
    }
}