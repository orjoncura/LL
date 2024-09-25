namespace LL.Data.Model;

public class SeminarWord
{
    public int Id { get; set; }
    
    public int WordId { get; set; }
    public virtual Word? Word { get; set; }
    
    public int SeminarId { get; set; }
    public virtual Seminar? Seminar { get; set; }
    
    public int SeminarWordRankId { get; set; }
    public virtual SeminarWordRank? SeminarWordRank { get; set; }
    
    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}