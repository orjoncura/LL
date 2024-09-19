namespace LL.Core.Models.ViewModels;

public class StatementViewModel
{
    public int Id { get; set; }
    public int SeminarWordId { get; set; }
    public string OriginalStatement { get; set; } = string.Empty;
    public string TranslatedStatement { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int CreatedById { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}