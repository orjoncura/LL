using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;

namespace LL.Core.Interfaces.Services
{
    public interface ICourseService
    {
        Task<List<WordViewModel>> CreateCourse(CourseRequestModel courseRequest, int userId);
        Task<List<ExerciseViewModel>> CreateExercises(int courseId, int userId);
    }
}
