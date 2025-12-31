using System.Transactions;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;
using LL.Resources.Contexts;
using LL.Resources.Factories;
using LL.Resources.Models;
using Microsoft.EntityFrameworkCore;

namespace LL.Resources.Repositories;

public class WordDifficultyRepository(AppDbContext db, IEncryptionService encryptionService) : IWordDifficultyRepository
{
    public List<WordViewModel> GetWordsByDifficulty(int importanceRatingId, int languageId, int userId)
    {
        var wordIds = 
            db.WordDifficultyLogs
                .Where(w =>
                    w.CreatedById == userId
                    && w.Word.ImportanceRatingId == importanceRatingId 
                    && w.Word.LanguageId == languageId
                    && w.IsActive
                    && w.Word.IsActive)
                .Select(w => w.WordId)
                .Distinct().ToList();
        
        return db.WordLinks
            .Include(w => w.Word)
            .Where(w => wordIds.Contains(w.WordId)).ToList()
            .Select(wl => DataFactory.Convert(encryptionService.Encrypt(wl.WordId), wl)).ToList();
    }
    public int GetWordCountByDifficulty(int importanceRatingId, int languageId, int userId)
    {
        return db.WordDifficultyLogs.Where(w =>
                w.CreatedById == userId
                && w.Word.ImportanceRatingId == importanceRatingId 
                && w.Word.LanguageId == languageId
                && w.IsActive
                && w.Word.IsActive)
            .Select(w => w.WordId)
            .Count();
    }
    public bool SetWordDifficulty(int difficultyId, string wordIdEncrypted, int userId)
    {
        int wordId = encryptionService.Decrypt(wordIdEncrypted);
        
        var wordDifficulty = db.WordDifficulties
            .FirstOrDefault(w => w.WordId == wordId  && w.UserId == userId && w.IsActive);
        
        using (var scope = new TransactionScope())
        {
            if (wordDifficulty == null)
            {
                wordDifficulty = new WordDifficulty()
                {
                    WordId = wordId,
                    UserId = userId,
                    DifficultyId = difficultyId,
                    IsActive = true
                };

                db.WordDifficulties.Add(wordDifficulty);
                db.SaveChanges();
            }
            else
            {
                wordDifficulty.DifficultyId = difficultyId;                    
                db.WordDifficulties.Update(wordDifficulty);
                db.SaveChanges();
            }

            var wordDifficultyLog = new WordDifficultyLog()
            {
                WordDifficultyId =  wordDifficulty.Id,
                WordId = wordDifficulty.WordId,
                UserId = wordDifficulty.UserId,
                DifficultyId = wordDifficulty.DifficultyId,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
                IsActive = wordDifficulty.IsActive
            };

            db.Add(wordDifficultyLog);
            db.SaveChanges();

            scope.Complete();
        }
        
        return true;
    }
}