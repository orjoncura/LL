using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Repositories;
public interface IWordRepository
{
    List<WordViewModel> GetMostImportantWords(int languageId);
    MemoryStream? GetFileStreamById(int wordId);
    WordShort Insert(string name, int languageId, int userId);
}

