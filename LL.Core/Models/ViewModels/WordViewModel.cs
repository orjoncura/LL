using LL.Core.Enums;

namespace LL.Core.Models.ViewModels;

public class WordViewModel
{
    public int Id {  get; set; }
    public string Name { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public int TypeId { get; set; }
    public int LanguageId { get; set; }
    public bool IsActive { get; set; }
    public int CreatedById { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

    public WordViewModel() { }

    public WordViewModel(string name, string definition, int typeId)
    {
        Name = name;
        Definition = definition;
        TypeId = typeId;
    }
}