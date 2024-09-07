namespace LL.SharedDefinitions.Models
{
    public class SeminarRequestModel
    {
        public List<string> Words { get; set; } = new List<string>();
        public int LanguageIdFrom { get; set; }
        public int LangaugeIdTo { get; set; }
    }
}
