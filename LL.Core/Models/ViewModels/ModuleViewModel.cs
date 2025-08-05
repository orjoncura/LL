namespace LL.Core.Models.ViewModels;

public class ModuleViewModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int TypeId { get; set; }
    public bool Unlocked { get; set; }
    public bool Completed { get; set; }
}