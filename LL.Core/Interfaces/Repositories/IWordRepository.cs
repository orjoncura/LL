using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Repositories;
public interface IWordRepository
{
    int Insert(string name, string definition, int typeId, int languageId, int userId);
}

