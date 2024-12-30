using LL.Core.Models.Short;

namespace LL.Core.Interfaces.Repositories;

public interface IWordLinkRepository
{
    WordLinkShort GetById(int id);
    WordLinkShort Insert(int wordId, string word, int fromId, int toId, int userId);
}