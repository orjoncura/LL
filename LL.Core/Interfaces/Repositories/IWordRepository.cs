using LL.Core.Models.Short;

namespace LL.Core.Interfaces.Repositories;
public interface IWordRepository
{
    MemoryStream? GetFileStreamById(int wordId);
    WordShort Insert(string name, int languageId, int userId);
}

