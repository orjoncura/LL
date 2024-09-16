using LL.Data.Contexts;
using LL.Data.Interfaces;
using LL.Data.Model;

namespace LL.Data.Repositories
{
    public class StatementRepository(AppDBContext appDBContext) : IStatementRepository
    {
        public List<Statement> GetByWordId(int wordId) =>
            appDBContext.Statements
                .Where(s => s.SeminarWord.WordId == wordId).ToList();
        
        public int Insert(int wordId, string original, string translated, int fromId, int toId, int userId)
        {
            if (string.IsNullOrEmpty(original) && string.IsNullOrEmpty(translated))
                return 0;
            
            var statement = new Statement
            {
                OriginalStatement = original,
                TranslatedStatement = translated,  
                IsActive = true,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };

            appDBContext.Add(statement);
            appDBContext.SaveChanges();

            return statement.Id;
        }

        public List<int> InsertRange(List<Statement> statements)
        {
            statements = statements
                .Where(s => string.IsNullOrWhiteSpace(s.OriginalStatement) == false 
                && string.IsNullOrWhiteSpace(s.TranslatedStatement) == false)
                .ToList();

            appDBContext.AddRange(statements);
            appDBContext.SaveChanges();

            return statements.Select(s => s.Id).ToList();
        }
    }
}
