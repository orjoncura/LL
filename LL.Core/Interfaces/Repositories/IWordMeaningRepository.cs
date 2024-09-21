namespace LL.Core.Interfaces.Repositories;

public interface IWordMeaningRepository
{
    bool Any(int wordId);
    int Insert(int wordId, int typeId, int userId);
}