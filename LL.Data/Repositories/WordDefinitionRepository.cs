using LL.Core.Interfaces.Repositories;
using LL.Data.Contexts;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class WordDefinitionRepository(AppDBContext appDBContext) : IWordDefinitionRepository 
{
    public int Insert(string definition, int wordMeaningId, int userId)
    {
        var wordDefinition = new WordDefinition()
        {
            Value = definition,
            WordMeaningId = wordMeaningId,
            IsActive = true,
            CreatedById = userId,
            CreatedDate = DateTime.Now,
        };

        appDBContext.Add(wordDefinition);
        appDBContext.SaveChanges();
        
        return wordDefinition.Id;
    }
}