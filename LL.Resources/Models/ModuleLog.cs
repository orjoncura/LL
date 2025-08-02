namespace LL.Resources.Models;

public class ModuleLog
{
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public virtual Module? Module { get; set; }
    public int CourseId { get; set; }
    public virtual Course? Course { get; set; }
    public string Title { get; set; }
    public int TypeId { get; set; }
    public virtual ModuleType? Type { get; set; }
    public bool Unlocked { get; set; }
    public bool Completed { get; set; }    
    public bool IsActive { get; set; }
    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }    
}