namespace LL.Core.Models.Arguments;

public class ExerciseRequestModel
{
    public int CourseId { get; set; }
    public int LanguageFromId { get; set; }
    //public virtual string LanguageFrom => EnumHelper.GetEnumValueById<LanguageEnum>(LanguageFromId);
    public int LanguageToId { get; set; }
    //public virtual string LanguageTo => EnumHelper.GetEnumValueById<LanguageEnum>(LanguageToId);
    
    public string Text { get; set; } = string.Empty;
    
    public int WordId {  get; set; }
    
    public string WordName { get; set; } = string.Empty;
    
    public int RankId { get; set; }
    
    public bool IsValid => 
        string.IsNullOrWhiteSpace(WordName) == false 
        && CourseId > 0 
        && WordId > 0 
        && RankId > 0;
}

