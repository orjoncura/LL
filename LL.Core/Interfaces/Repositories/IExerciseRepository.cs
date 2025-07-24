
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Repositories;

public interface IExerciseRepository
{
    List<ExerciseViewModel> GetByCourseId(int courseWordId);
    List<int> InsertRange(int seminarWordId, List<ExerciseViewModel> statementShorts, int userId);
}

