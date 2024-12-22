using LL.Core.Models.Short;

namespace LL.Core.Interfaces.Repositories;

public interface IWordMeaningRepository
{
    public List<MeaningShort>? GetByWordId(int wordId);
    int Insert(int wordId, int typeId, int userId);
}