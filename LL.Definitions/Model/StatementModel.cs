using LL.Data.Model;

namespace LL.SharedDefinitions.Model
{
    public class StatementModel
    {
        public string OriginalStatement { get; set; } = string.Empty;
        public string TranslatedStatement { get; set; } = string.Empty;

        public StatementModel() { }
        public StatementModel(Statement statement) 
        {
            OriginalStatement = statement.OriginalStatement;
            TranslatedStatement = statement.OriginalStatement;
        }

        public StatementModel(string original, string translated) 
        {
            OriginalStatement = original;
            TranslatedStatement = translated;
        }
    }
}
