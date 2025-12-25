namespace LL.Resources.Models;

public class WordDifficultyLog
{
    public int Id { get; set; }
    public int WordDifficultyId { get; set; }
    public virtual WordDifficulty? WordDifficulty { get; set; }   
    public int WordId { get; set; }
    public virtual Word? Word { get; set; }    
    public int UserId { get; set; }
    public virtual User? User { get; set; }
    public int DifficultyId { get; set; }
    public virtual ImportanceRating? Difficulty { get; set; }    
    public bool  IsActive { get; set; }    
    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }    
}