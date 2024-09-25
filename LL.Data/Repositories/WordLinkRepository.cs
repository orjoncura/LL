using LL.Core.Interfaces.Repositories;
using LL.Data.Contexts;
using LL.Data.Model;

namespace LL.Data.Repositories;

public class WordLinkRepository(AppDBContext db) : IWordLinkRepository
{
    private WordLink? Get(int sourceId, int targetId) =>
        db.WordLinks.FirstOrDefault(w => w.SourceId == sourceId && w.TargetId == targetId && w.IsActive);
    
    public int Insert(int wordId, int translatedWordId, int userId)
    {
        var wordLink = Get(wordId, translatedWordId);

        if (wordLink == null)
        {
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
        }

        return wordLink.Id;
    }
}