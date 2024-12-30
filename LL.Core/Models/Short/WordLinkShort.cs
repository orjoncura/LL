namespace LL.Core.Models.Short;

public class WordLinkShort
{
    public int Id {  get; set; }
    
    public WordShort Source { get; set; } = null!;
    
    public WordShort Target { get; set; } = null!;
}