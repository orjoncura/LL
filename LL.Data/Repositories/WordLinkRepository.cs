using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.Short;
using LL.Data.Contexts;
using LL.Data.Factories;
using LL.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace LL.Data.Repositories;

public class WordLinkRepository(AppDbContext db,
    ITranslationService translationService,
    IWordRepository wordRepository) : IWordLinkRepository
{
    public WordLinkShort GetById(int id)
    {
        WordLink wordLink = db.WordLinks
            .Include(w => w.Source)
            .Include(w => w.Target)
            .FirstOrDefault(w => w.Id == id && w.IsActive);

        if (wordLink == null)
            return new WordLinkShort();
        
        return new WordLinkShort()
        {
            Id = wordLink.Id,
            Source = DataFactory.Convert(wordLink.Source),
            Target = DataFactory.Convert(wordLink.Target)
        };
    }
    
    public WordLinkShort Insert(int wordId, string word, int fromId, int toId, int userId)
    {
        WordLink wordLink = db.WordLinks
            .Include(w => w.Target)
            .Include(w => w.Source)
            .FirstOrDefault(w => 
                w.SourceId == wordId 
                && w.Target.LanguageId == toId 
                && w.Target.IsActive
                && w.IsActive);

        if (wordLink == null)
        {
            string translatedWord = translationService.TranslateText(word, fromId, toId).Result;
            WordShort wordShort = wordRepository.Insert(translatedWord, toId, userId);
            
            wordLink = new WordLink()
            {
                SourceId = wordId,
                TargetId = wordShort.Id,
                IsActive = true,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };

            db.Add(wordLink);
            db.SaveChanges();
        }
        
        return new WordLinkShort()
        {
            Id = wordLink.Id,
            Source = DataFactory.Convert(wordLink.Source),
            Target = DataFactory.Convert(wordLink.Target)
        };
    }
    
}