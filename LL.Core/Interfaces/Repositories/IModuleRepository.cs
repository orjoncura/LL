using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Repositories;

public interface IModuleRepository
{
    List<ModuleViewModel> GetByCourseId(string courseId);
    bool ModuleCreated(int courseId, int typeId);
    int Insert(int courseId, string title, int typeId, int sequence, bool unlocked, int loginId);
    bool MarkModuleAsComplete(string moduleId);
}