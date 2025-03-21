namespace LL.Core.Models.Short;

public class WordLinkShort
{
    public int Id {  get; set; }
    
    public WordShort Source { get; set; } = null!;

    public string Translation { get; set; } = null!;
    public int LanguageId {  get; set; }
}