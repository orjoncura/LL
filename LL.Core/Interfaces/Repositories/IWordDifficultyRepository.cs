namespace LL.Core.Interfaces.Repositories;

public interface IWordDifficultyRepository
{
    bool SetWordDifficulty(int difficultyId, string wordId, int userId);
}