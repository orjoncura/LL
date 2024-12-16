using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Data.Contexts;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class WordLinkRepository(AppDBContext db,
    ITranslationService translationService,
    IWordRepository wordRepository) : IWordLinkRepository
{
    public int Insert(int wordId, string word, int fromId, int toId, int userId)
    {
        WordLink wordLink = db.WordLinks
            .FirstOrDefault(w => 
                w.SourceId == wordId 
                && w.Target.LanguageId == toId 
                && w.Target.IsActive
                && w.IsActive);

        if (wordLink == null)
        {
            string translatedWord = translationService.TranslateText(word, fromId, toId).Result;
            int translatedWordId = wordRepository.Insert(translatedWord, toId, userId);
            
            wordLink = new WordLink()
            {
                SourceId = wordId,
                TargetId = translatedWordId,
                IsActive = true,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };

            db.Add(wordLink);
            db.SaveChanges();
            
            return wordLink.Id;
        }
        
        return wordLink.Id;
    }
}