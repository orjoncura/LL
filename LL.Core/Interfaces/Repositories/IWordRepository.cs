using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Repositories;
public interface IWordRepository
{
    int Insert(string name, int languageId, int userId);
}

