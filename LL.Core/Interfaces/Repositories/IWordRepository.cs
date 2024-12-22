using LL.Core.Models.Short;

namespace LL.Core.Interfaces.Repositories;
public interface IWordRepository
{
    WordShort Insert(string name, int languageId, int userId);
}

