using LL.Core.Enums;
using LL.Core.Helpers;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.Short;
using LL.Data.Contexts;
using LL.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace LL.Data.Repositories;

public class WordMeaningRepository(AppDbContext db) : IWordMeaningRepository
{
    public List<MeaningShort>? GetByWordId(int wordId) 
    {
        var meanings = db.WordMeanings
            .Include(w => w.Type)
            .Include(w => w.WordDefinitions)
            .Where(w => w.WordId == wordId & w.IsActive).ToList();

        if (meanings.Any())
        {
            return meanings.Select(m => new MeaningShort()
            {
                Type = m.Type.Value,
                Definitions = m.WordDefinitions.Select(w => w.Value).ToList()
            }).ToList();
        }
        
        return null;
    }
        
    
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