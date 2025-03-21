using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.Arguments;
using LL.Core.Models.Short;
using LL.Data.Contexts;
using LL.Data.Factories;
using LL.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace LL.Data.Repositories;

public class WordLinkRepository(AppDbContext db) : IWordLinkRepository
{
    public WordLinkShort GetById(int id)
    {
        WordLink wordLink = db.WordLinks
            .Include(w => w.Word)
            .FirstOrDefault(w => w.Id == id && w.IsActive);

        if (wordLink == null)
            return new WordLinkShort();
        
        return new WordLinkShort()
        {
            Id = wordLink.Id,
            Source = DataFactory.Convert(wordLink.Word),
            Translation = wordLink.Value,
            LanguageId = wordLink.LanguageId
        };
    }
    
    public WordLinkShort Insert(int wordId, DefinitionRequestModel definitionRequestModel, int userId)
    {
        WordLink wordLink = db.WordLinks
            .Include(w => w.Word)
            .FirstOrDefault(w => 
                w.WordId == wordId 
                && w.LanguageId == definitionRequestModel.LanguageToId 
                && w.Word.LanguageId == definitionRequestModel.LanguageFromId 
                && w.Word.IsActive
                && w.IsActive);

        if (wordLink == null)
        {
            wordLink = new WordLink()
            {
                WordId = wordId,
                Value = definitionRequestModel.Translation,
                LanguageId = definitionRequestModel.LanguageToId,
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
            Source = DataFactory.Convert(wordLink.Word),
            Translation = wordLink.Value,
            LanguageId = wordLink.LanguageId
        };
    }
    
}