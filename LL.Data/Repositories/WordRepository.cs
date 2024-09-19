using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.ViewModels;
using LL.Data.Model;

namespace LL.Data.Repositories;
public class WordRepository(AppDBContext appDBContext) : IWordRepository
{
    public int Insert(string name, string definition, int typeId, int languageId, int userId)
    {
        var word = GetSingleByName(name, languageId);
        
        if (word == null)
        {
            word = new Word
            { 
                Name = name.Trim(),
                Definition = definition.Trim(),
                LanguageId = languageId,
                TypeId = typeId,
                IsActive = true,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };

            appDBContext.Add(word);
            appDBContext.SaveChanges();
        }
        
        return word.Id;
    }

    private Word? GetSingleByName(string name, int fromId) => 
        appDBContext.Words.FirstOrDefault(w => 
            w.Name.ToLower() == name.Trim().ToLower()
            & w.LanguageId == fromId);
}

