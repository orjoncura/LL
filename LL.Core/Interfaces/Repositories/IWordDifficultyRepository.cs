using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Repositories;

public interface IWordDifficultyRepository
{    
    List<WordViewModel> GetWordsByDifficulty(int importanceRatingId, int languageId, int userId);
    int GetWordCountByDifficulty(int importanceRatingId, int languageId, int userId);
    bool SetWordDifficulty(int difficultyId, string wordId, int userId);
}