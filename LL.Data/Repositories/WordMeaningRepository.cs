using LL.Core.Interfaces.Repositories;
using LL.Core.Models.Short;
using LL.Data.Contexts;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class WordMeaningRepository(AppDBContext db) : IWordMeaningRepository
{
    public bool Any(int wordId) =>
        db.WordMeanings
            .Any(w => w.WordId == wordId & w.IsActive);
    
    public int Insert(int wordId, int typeId, int userId)
    {
        var wordMeaning = new WordMeaning()
        {
            WordId = wordId,
            TypeId = typeId,
            IsActive = true,
            CreatedById = userId,
            CreatedDate = DateTime.Now,
        };

        db.Add(wordMeaning);
        db.SaveChanges();
        
        return wordMeaning.Id;
    }
}