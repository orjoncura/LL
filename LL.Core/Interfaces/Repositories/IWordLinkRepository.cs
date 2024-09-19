namespace LL.Core.Interfaces.Repositories;

public interface IWordLinkRepository
{
    int Insert(int wordId, int translatedWordId, int userId);
}