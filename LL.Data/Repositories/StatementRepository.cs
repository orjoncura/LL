using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;
using LL.Data.Model;

namespace LL.Data.Repositories;
public class StatementRepository(AppDBContext appDBContext) : IStatementRepository
{
    public int Insert(int seminarWordId, string original, string translated, int userId)
    {
        if (string.IsNullOrEmpty(original) && string.IsNullOrEmpty(translated))
            return 0;
        
        var statement = new Statement
        {
            SeminarWordId = seminarWordId,
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

    public List<int> InsertRange(int seminarWordId, List<StatementShort> statementShorts, int userId)
    {
        var statements = new List<Statement>();
        
        foreach (StatementShort s in statementShorts.Where(s => s.IsValid).ToList())
        {
            var statement = new Statement
            {
                SeminarWordId = seminarWordId,
                OriginalStatement = s.OriginalStatement,
                TranslatedStatement = s.TranslatedStatement,
                IsActive = true,
                CreatedById = userId
            };
            
            statements.Add(statement);
        }

        if (statements.Any())
        {
            appDBContext.AddRange(statements);
            appDBContext.SaveChanges();
        }

        return statements.Select(s => s.Id).ToList();
    }
}

