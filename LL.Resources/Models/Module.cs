namespace LL.Resources.Models;

public class Module
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public virtual Course? Course { get; set; }
    public string Title { get; set; }
    public int TypeId { get; set; }
    public virtual ModuleType? Type { get; set; }
    public bool Unlocked { get; set; }
    public bool Completed { get; set; }    
    public int Sequence { get; set; }
    public bool IsActive { get; set; }
    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }    
}