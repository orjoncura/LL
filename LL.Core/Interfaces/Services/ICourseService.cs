using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Services
{
    public interface ICourseService
    {
        Task<CourseViewModel> CreateCourse(CourseRequestModel courseRequest, int userId);
        Task<WordViewModel> CreateDefinitions(DefinitionRequestModel definitionRequestModel, int userId);
        Task<List<ExerciseViewModel>> CreateExercises(ExerciseRequestModel exerciseRequest, int userId);
    }
}
