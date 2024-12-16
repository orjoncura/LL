namespace LL.Core.Interfaces.Repositories;

public interface IWordLinkRepository
{
    int Insert(int wordId, string word, int fromId, int toId, int userId);
}