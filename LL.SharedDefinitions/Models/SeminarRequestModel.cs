namespace LL.SharedDefinitions.Models
{
    public class SeminarRequestModel
    {
        public List<string> Words { get; set; } = new List<string>();
        public int LanguageFromId { get; set; }
        public int LangaugeToId { get; set; }

        public bool IsValid => Words.Any() && LanguageFromId > 0 && LangaugeToId > 0;
    }
}
