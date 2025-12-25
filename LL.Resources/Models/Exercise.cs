namespace LL.Resources.Models;

public class Exercise
{
    public int Id { get; set; }

    public int ModuleId { get; set; }
    public virtual Module? Module { get; set; }
    public string Original { get; set; } = string.Empty;
    public string Translated { get; set; } = string.Empty;
    public string Extra { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public int CreatedById { get; set; }
    public virtual User? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public int? UpdatedById { get; set; }
    public virtual User? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

