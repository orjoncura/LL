using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Repositories;

public interface IModuleRepository
{
    List<ModuleViewModel> GetByCourseId(string courseId);

    int Insert(int courseId, string title, int typeId, bool unlocked, int loginId);
}