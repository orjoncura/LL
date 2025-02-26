using LL.Core.Enums;
using LL.Core.Helpers;
using LL.Core.Interfaces.Extensions;
using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.DataTransferObjects;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;
using LL.Data.Factories;
using LL.Data.Model;
using Microsoft.EntityFrameworkCore;
using LL.Core.Models.Arguments;

namespace LL.Data.Repositories;
public class WordRepository(AppDbContext db,
    StorageModel storageModel, 
    IStorageService storageService, 
    ITextToSpeechService textToSpeechService) : IWordRepository
{
    public List<WordViewModel> GetKeyWords(int languageId)
    {
        var words = db.WordLinks
            .Where(w => w.Source.LanguageId == languageId
                        && (w.Source.ImportanceRatingId == (int)ImportanceRatingEnum.Medium 
                            || w.Source.ImportanceRatingId == (int)ImportanceRatingEnum.High)
                        && w.IsActive)
            .Include(wordLink => wordLink.Source)
            .Include(wordLink => wordLink.Target).ToList()
            .Select(w => new WordViewModel(DataFactory.Convert(w.Source), w.Target.Name)).ToList();
        
        return words;
    }
    public MemoryStream? GetFileStreamById(int wordId)
    {
        var word = db.Words.FirstOrDefault(w => w.Id == wordId);

        if (word == null)
            return null;
        
        string audioPath = word.AudioPath;
        
        var memoryStream = new MemoryStream();
        // Write audio data to memoryStream (replace with actual logic)
        memoryStream.Write(storageService.GetFile(storageModel, audioPath).Result);
        memoryStream.Seek(0, SeekOrigin.Begin);

        // Return memory stream as content
        return memoryStream;
    }
    
    public WordShort Insert(string name, int languageId, int userId)
    {
        Word? word = GetSingleByName(name, languageId);
        
        if (word == null)
        {
            word = new Word
            { 
                Name = name.Trim().ToLower(),
                AudioPath = storageService
                    .SaveFile(storageModel, textToSpeechService.CreateAudio(name, (LanguageEnum)languageId)).Result,
                LanguageId = languageId,
                ImportanceRatingId = (int)ImportanceRatingEnum.Low,
                IsActive = true,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };

            db.Add(word);
            db.SaveChanges();
        }
        
        return  DataFactory.Convert(word);
    }

    private Word? GetSingleByName(string name, int fromId) => 
        db.Words.FirstOrDefault(w => 
            w.Name.ToLower() == name.Trim().ToLower()
            & w.LanguageId == fromId);
    
}

