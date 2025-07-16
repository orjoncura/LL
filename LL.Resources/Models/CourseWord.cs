namespace LL.Resources.Models;

public class CourseWord
{
    public int Id { get; set; }
    
    public int WordId { get; set; }
    public virtual Word? Word { get; set; }
    
    public int CourseId { get; set; }
    public virtual Course? Course { get; set; }
    public int ImportanceRatingId { get; set; }
    public virtual ImportanceRating? ImportanceRating { get; set; }
    
    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    
    public int? UpdatedById { get; set; }
    public virtual User? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
}