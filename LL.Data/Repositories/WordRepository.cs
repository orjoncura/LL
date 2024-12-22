using LL.Core.Enums;
using LL.Core.Helpers;
using LL.Core.Interfaces.Extensions;
using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.DataTransferObjects;
using LL.Core.Models.Short;
using LL.Data.Model;

namespace LL.Data.Repositories;
public class WordRepository(AppDBContext db,
    StorageModel storageModel, 
    IStorageService storageService, 
    ITextToSpeechService textToSpeechService) : IWordRepository
{
    public WordShort Insert(string name, int languageId, int userId)
    {
        Word? word = GetSingleByName(name, languageId);
        
        byte[] file = word == null 
            ? textToSpeechService.CreateAudio(name, languageId) 
            : storageService.GetFile(storageModel, word.AudioPath).Result;
        
        if (word == null)
        {
            word = new Word
            { 
                Name = name.Trim(),
                AudioPath = storageService.SaveFile(storageModel, file).Result,
                LanguageId = languageId,
                IsActive = true,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };

            db.Add(word);
            db.SaveChanges();
        }
        
        WordShort wordShort = new WordShort()
        {
            Id = word.Id,
            Name = word.Name,
            Audio = file,
            Language = EnumHelper.GetEnumValueById<LanguageEnum>(word.LanguageId)
        };
        
        return wordShort;
    }

    private Word? GetSingleByName(string name, int fromId) => 
        db.Words.FirstOrDefault(w => 
            w.Name.ToLower() == name.Trim().ToLower()
            & w.LanguageId == fromId);
    
}

