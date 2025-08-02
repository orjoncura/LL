
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Repositories;

public interface IExerciseRepository
{
    List<ExerciseViewModel> GetByModuleId(string moduleId);
    List<int> InsertRange(string moduleId, List<ExerciseViewModel> statementShorts, int userId);
}

