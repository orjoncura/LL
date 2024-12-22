using LL.Core.Interfaces.Repositories;
using LL.Data.Contexts;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class WordDefinitionRepository(AppDBContext db) : IWordDefinitionRepository 
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