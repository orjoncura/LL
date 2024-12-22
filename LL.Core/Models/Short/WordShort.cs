namespace LL.Core.Models.Short;

public class WordShort
{
    public int Id {  get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public byte[] Audio { get; set; } = [];
    
    public string Language { get; set; }
}