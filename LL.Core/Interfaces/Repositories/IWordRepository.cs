using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;
using LL.Core.Models.Arguments;

namespace LL.Core.Interfaces.Repositories;
public interface IWordRepository
{
    List<WordViewModel> GetKeyWords(int languageId);
    MemoryStream? GetFileStreamById(int wordId);
    WordShort Insert(string name, int languageId, int userId);
    List<WordViewModel> GetByCourseId(int id);
}

