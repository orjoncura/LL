using LL.Core.Interfaces.Repositories;
using LL.Resources.Contexts;
using LL.Resources.Models;

namespace LL.Resources.Repositories;

public class WordDefinitionRepository(AppDbContext db) : IWordDefinitionRepository 
{
    public int Insert(string definition, int wordMeaningId, int userId)
    {
        WordDefinition? wordDefinition = db.WordDefinitions.FirstOrDefault(wd => wd.WordMeaningId == wordMeaningId);
        
        if (wordDefinition == null)
        {
             wordDefinition = new WordDefinition()
            {
                Value = definition,
                WordMeaningId = wordMeaningId,
                IsActive = true,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };

            db.Add(wordDefinition);
            db.SaveChanges();
        }

        
        return wordDefinition.Id;
    }
}