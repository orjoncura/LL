using LL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.SharedDefinitions.Model
{
    public class SeminarViewModel
    {
        public StatementModel TargetWord { get; set; } = new StatementModel();

        public List<StatementModel> Sentences { get; set; } = new List<StatementModel>();

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
                    LanguageFromId = fromId,
                    LanguageToId = toId,
                    CreatedById = userId,
                    CreatedDate = DateTime.Now,
                };

                statements.Add(statement);
            }

            return statements;
        }
    }
}
