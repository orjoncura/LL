using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Repositories;

public interface IModuleRepository
{
    int Insert(int courseId, string title, int typeId, bool unlocked, int loginId);
    List<ModuleViewModel> GetByCourseId(string courseId);
    bool MarkModuleAsComplete(string moduleId);
}