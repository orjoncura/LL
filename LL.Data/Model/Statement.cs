namespace LL.Data.Model
{
    public class Statement
    {
        public int Id { get; set; }
        public int WordId { get; set; }
        public virtual Word Word { get; set; } = new Word();

        public string OriginalStatement { get; set; } = string.Empty;
        public string TranslatedStatement { get; set; } = string.Empty;

        public int LanguageFromId { get; set; }
        public virtual Language LanguageFrom { get; set; } = new Language();

        public int LanguageToId { get; set; }
        public virtual Language LanguageTo { get; set; } = new Language();

        public bool IsActive { get; set; }

        public int CreatedById { get; set; }
        public virtual Person CreatedBy { get; set; } = new Person();
        public DateTimeOffset CreatedDate { get; set; }
    }
}
