namespace LL.Resources.Models;

public class Word 
{
    public int Id {  get; set; }
    public string Name { get; set; } = string.Empty;
    
    public int DocumentId { get; set; }
    public virtual ContentEntry? Document { get; set; }
    public int LanguageId { get; set; }
    public virtual Language? Language { get; set; }
    public int ImportanceRatingId { get; set; }
    public virtual ImportanceRating? ImportanceRating { get; set; }
    public bool IsActive { get; set; }
    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public virtual List<WordMeaning> WordMeanings { get; set; }
}

