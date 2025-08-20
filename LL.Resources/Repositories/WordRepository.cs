using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Resources.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.DataTransferObjects;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;
using LL.Resources.Factories;
using LL.Resources.Models;
using Microsoft.EntityFrameworkCore;

namespace LL.Resources.Repositories;
public class WordRepository(AppDbContext db,
    StorageModel storageModel, 
    IStorageService storageService, 
    ITextToSpeechService textToSpeechService,
    IEncryptionService encryptionService) : IWordRepository
{
    
    public List<WordViewModel> GetRangeByText(List<string> names, int languageId)
    {
        var words = db.WordLinks
            .Include(w => w.Word)
            .Include(w => w.Word.WordMeanings)            
            .ThenInclude(w => w.WordDefinitions)
            .Include(w => w.Word.WordMeanings)
            .ThenInclude(w => w.Type)
            .Where(w => names.Contains(w.Word.Name) && w.Word.LanguageId == languageId && w.IsActive).ToList()
            .Select(wl => DataFactory.Convert(encryptionService.Encrypt(wl.WordId), wl)).ToList();
        
        return words;
    }
    public List<WordViewModel> GetKeyWords(int languageId)
    {
        var words = db.WordLinks
            .Include(w => w.Word)
            .Include(w => w.Word.WordMeanings)            
            .ThenInclude(w => w.WordDefinitions)
            .Include(w => w.Word.WordMeanings)
            .ThenInclude(w => w.Type)
            .Where(w => w.Word.LanguageId == languageId
                && (w.Word.ImportanceRatingId == (int)ImportanceRatingEnum.Medium 
                    || w.Word.ImportanceRatingId == (int)ImportanceRatingEnum.High)
                && w.IsActive).ToList()
            .Select(wl => DataFactory.Convert(encryptionService.Encrypt(wl.WordId), wl)).ToList();
        
        return words;
    }
    public MemoryStream? GetFileStreamById(string wordId)
    {
        var word = db.Words.FirstOrDefault(w => w.Id == encryptionService.Decrypt(wordId));

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
                ImportanceRatingId = (int)ImportanceRatingEnum.Medium,
                IsActive = true,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };

            db.Add(word);
            db.SaveChanges();
        }
        
        return  DataFactory.Convert(word);
    }
    
    public List<WordViewModel> GetByModuleId(string id)
    {
        int moduleId = encryptionService.Decrypt(id);
        
        List<WordViewModel> words =
            (from courseWord in db.CourseWords
                join wordLink in db.WordLinks
               on courseWord.WordId equals wordLink.WordId
             where courseWord.ModuleId == moduleId 
                   && courseWord.IsActive
                   && courseWord.Module.IsActive
                   && wordLink.IsActive  
                   && wordLink.Word.IsActive 
                   && wordLink.Word.IsActive 
             select wordLink)
            .Include(w => w.Word.WordMeanings)
            .ThenInclude(w => w.WordDefinitions)
            .Include(w => w.Word.WordMeanings)
            .ThenInclude(w => w.Type)
            .ToList().Select(wl => DataFactory.Convert(encryptionService.Encrypt(wl.WordId), wl)).ToList();
        
        return words;
    }
    
    private Word? GetSingleByName(string name, int fromId) => 
        db.Words.FirstOrDefault(w => 
            w.Name.ToLower() == name.Trim().ToLower()
            & w.LanguageId == fromId);
    
}

