using LL.Core.Constants;
using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace LL.Test.Repositories;

public class ExerciseRepositoryTest
{
    private readonly ICourseRepository _courseRepository;
    private readonly IModuleRepository _moduleRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IEncryptionService _encryptionService;

    public ExerciseRepositoryTest()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c[Secrets.EncryptionKey])
            .Returns("edTWS52cRCrRB4NDDCwT6mY6dMcWwa3n");

        var services = Provider.GetRequiredService();
        services.RemoveAll<IConfiguration>();
        services.AddSingleton(mockConfiguration.Object);

        var serviceProvider = services.BuildServiceProvider();
        _courseRepository = serviceProvider.GetRequiredService<ICourseRepository>();
        _moduleRepository = serviceProvider.GetRequiredService<IModuleRepository>();
        _exerciseRepository = serviceProvider.GetRequiredService<IExerciseRepository>();
        _encryptionService = serviceProvider.GetRequiredService<IEncryptionService>();
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

    [Fact]
    public void GetByModuleId_ShouldReturnInsertedExercises()
    {
        var courseId = _courseRepository.Insert(
            "Exercise get",
            $"exercise-get-{Guid.NewGuid():N}",
            (int)LanguageEnum.Spanish,
            (int)LanguageEnum.English,
            1);
        var moduleId = _moduleRepository.Insert(courseId, "Exercises", (int)ModuleTypeEnum.Exercises, 1, true, 1);

        _exerciseRepository.InsertRange(moduleId, new List<ExerciseViewModel>
        {
            new()
            {
                Original = "Buenos dias",
                Translated = "Good morning",
                Extra = "Greeting"
            }
        }, 1);

        var exercises = _exerciseRepository.GetByModuleId(_encryptionService.Encrypt(moduleId));

        Assert.Contains(exercises, e => e.Original == "Buenos dias");
    }
}
