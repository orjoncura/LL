using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;
using LL.Core.Enums;

namespace LL.Core.Interfaces.Repositories;
public interface IWordRepository
{
    List<WordViewModel> GetRangeByText(List<string> names, int languageId, int userId);
    List<WordViewModel> GetKeyWords(int languageId);
    MemoryStream? GetFileStreamById(string wordId);
    WordShort Insert(string name, int languageId, int userId, int rating = (int)ImportanceRatingEnum.Medium);
    List<WordViewModel> GetByModuleId(string id);
}

