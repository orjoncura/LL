namespace LL.SharedDefinitions.Models
{
    public class SeminarRequestModel
    {
        public string Text { get; set; } = string.Empty;
        public int LanguageFromId { get; set; }
        public int LanguageToId { get; set; }

        public bool IsValid => 
            string.IsNullOrWhiteSpace(Text) == false 
            && LanguageFromId > 0 
            && LanguageToId > 0;
    }
}
