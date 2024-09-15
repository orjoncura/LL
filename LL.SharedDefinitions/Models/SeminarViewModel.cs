using LL.Data.Model;

namespace LL.SharedDefinitions.Models
{
    public class SeminarViewModel
    {
        public TargetWordViewModel TargetWord { get; set; } = new TargetWordViewModel();

        public List<StatementModel> Sentences { get; set; } = new List<StatementModel>();
        
        public bool IsValid => 
            string.IsNullOrWhiteSpace(TargetWord.Name) == false
            && string.IsNullOrWhiteSpace(TargetWord.Translation) == false
            && string.IsNullOrWhiteSpace(TargetWord.Definition) == false
            && string.IsNullOrWhiteSpace(TargetWord.Type) == false
            && Sentences.Any(s => string.IsNullOrWhiteSpace(s.OriginalStatement) == false
                                  && string.IsNullOrWhiteSpace(s.TranslatedStatement) == false);
        public List<Statement> ConvertToStatements(int wordId, int fromId, int toId, int userId)
        {
            var statements = new List<Statement>();

            foreach (var sentence in Sentences) 
            {
                var statement = new Statement
                {
                    WordId = wordId,
                    OriginalStatement = sentence.OriginalStatement,
                    TranslatedStatement = sentence.TranslatedStatement,
                    IsActive = true,
                    CreatedById = userId,
                    CreatedDate = DateTime.Now,
                };

                statements.Add(statement);
            }

            return statements;
        }
    }
}
