using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.ViewModels;
using LL.Data.Model;

namespace LL.Data.Repositories;
public class WordRepository(AppDBContext db) : IWordRepository
{
    public int Insert(string name, int languageId, int userId)
    {
        var word = GetSingleByName(name, languageId);
        
        if (word == null)
        {
            word = new Word
            { 
                Name = name.Trim(),
                LanguageId = languageId,
                IsActive = true,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };

            db.Add(word);
            db.SaveChanges();
        }
        
        return word.Id;
    }

    private Word? GetSingleByName(string name, int fromId) => 
        db.Words.FirstOrDefault(w => 
            w.Name.ToLower() == name.Trim().ToLower()
            & w.LanguageId == fromId);
}

