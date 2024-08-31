using LL.Data.Contexts;
using LL.Data.Model;

namespace LL.Data.Repositories
{
    public class StatementRepository(AppDBContext appDBContext)
    {
        public List<Statement> GetByWordId(int wordId) 
        {
            return appDBContext.Statements
                .Where(s => s.WordId == wordId).ToList();
        }

        public int Insert(int wordId, string original, string translated, int fromId, int toId, int userId)
        {
            var statement = new Statement
            {
                WordId = wordId,
                OriginalStatement = original,
                TranslatedStatement = translated,  
                LanguageFromId = fromId,
                LanguageToId = toId,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };

            appDBContext.Add(statement);

            return statement.Id;
        }
    }
}
