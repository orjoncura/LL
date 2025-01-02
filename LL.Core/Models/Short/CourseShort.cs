using LL.Core.Enums;
using LL.Core.Helpers;

namespace LL.Core.Models.Short;

public class CourseShort
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int LanguageFromId { get; set; }
    public virtual string LanguageFrom => EnumHelper.GetEnumValueById<LanguageEnum>(LanguageFromId);
    public int LanguageToId { get; set; }
    public virtual string LanguageTo => EnumHelper.GetEnumValueById<LanguageEnum>(LanguageToId);
    
    public string Text { get; set; } = string.Empty;
}