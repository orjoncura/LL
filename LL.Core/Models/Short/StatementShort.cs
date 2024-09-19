namespace LL.Core.Models.Short;

public class StatementShort
{
    public string OriginalStatement { get; set; } = string.Empty;
    public string TranslatedStatement { get; set; } = string.Empty;

    public bool IsValid =>
        string.IsNullOrWhiteSpace(OriginalStatement) == false
        && string.IsNullOrWhiteSpace(TranslatedStatement) == false;
    public StatementShort() { }
    public StatementShort(string original, string translated) 
    {
        OriginalStatement = original;
        TranslatedStatement = translated;
    }
}

