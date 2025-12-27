using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Repositories;
public interface IWordRepository
{
    List<WordViewModel> GetRangeByText(List<string> names, int languageId, int userId);
    List<WordViewModel> GetKeyWords(int languageId);
    MemoryStream? GetFileStreamById(string wordId);
    WordShort Insert(string name, int languageId, int userId);
    List<WordViewModel> GetByModuleId(string id);
}

