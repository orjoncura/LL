using LL.Core.Enums;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.ViewModels;

namespace LL.Test.Repositories;

public class ExerciseRepositoryTest
{
    private ICourseRepository _courseRepository { get; set; }
    private IModuleRepository _moduleRepository { get; set; }
    private IExerciseRepository _exerciseRepository { get; set; }

    public ExerciseRepositoryTest()
    {
        _courseRepository = Provider.GetRequiredService<ICourseRepository>();
        _moduleRepository = Provider.GetRequiredService<IModuleRepository>();
        _exerciseRepository = Provider.GetRequiredService<IExerciseRepository>();
    }

    [Fact]
    public void InsertRange_ShouldCreateExercises()
    {
        var courseId = _courseRepository.Insert(
            "Course",
            $"exercise-course-{Guid.NewGuid():N}",
            (int)LanguageEnum.English,
            (int)LanguageEnum.Spanish,
            1);
        var moduleId = _moduleRepository.Insert(courseId, "Exercises", (int)ModuleTypeEnum.Exercises, 1, true, 1);

        var ids = _exerciseRepository.InsertRange(moduleId, new List<ExerciseViewModel>
        {
            new()
            {
                Original = "Hello",
                Translated = "Hola",
                Extra = "Greeting"
            }
        }, 1);

        Assert.Single(ids);
    }
}
