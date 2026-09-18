using LL.Core.Constants;
using LL.Core.Enums;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace LL.Test.Repositories;

public class CourseRepositoryTest
{
    private readonly ICourseRepository _courseRepository;
    private readonly IModuleRepository _moduleRepository;
    private readonly IEncryptionService _encryptionService;

    public CourseRepositoryTest()
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
        _encryptionService = serviceProvider.GetRequiredService<IEncryptionService>();
    }

    [Fact]
    public void Insert_ShouldCreateCourse()
    {
        var value = $"test-value-{Guid.NewGuid():N}";
        var courseId = _courseRepository.Insert(
            "Test title",
            value,
            (int)LanguageEnum.English,
            (int)LanguageEnum.Spanish,
            1);

        Assert.True(courseId > 0);
    }

    [Fact]
    public void UpdateValue_ShouldUpdateCourseText()
    {
        var courseId = _courseRepository.Insert(
            "Pending course",
            $"pending:{Guid.NewGuid():N}",
            (int)LanguageEnum.Spanish,
            (int)LanguageEnum.English,
            1);

        var updated = _courseRepository.UpdateValue(courseId, "Updated course text", 1);
        var course = _courseRepository.GetById(_encryptionService.Encrypt(courseId));

        Assert.True(updated);
        Assert.Equal("Updated course text", course?.Text);
    }

    [Fact]
    public void MarkCourseAsCompleted_ShouldSetCompleted()
    {
        var courseId = _courseRepository.Insert(
            "Complete me",
            $"complete-{Guid.NewGuid():N}",
            (int)LanguageEnum.Spanish,
            (int)LanguageEnum.English,
            1);

        Assert.True(_courseRepository.MarkCourseAsCompleted(courseId));

        var course = _courseRepository.GetById(_encryptionService.Encrypt(courseId));
        Assert.True(course?.IsCompleted);
    }

    [Fact]
    public void DeleteCourseById_ShouldDeactivateCourse()
    {
        var courseId = _courseRepository.Insert(
            "Delete me",
            $"delete-{Guid.NewGuid():N}",
            (int)LanguageEnum.Spanish,
            (int)LanguageEnum.English,
            1);
        var encryptedId = _encryptionService.Encrypt(courseId);

        Assert.True(_courseRepository.DeleteCourseById(encryptedId, 1));
        Assert.Null(_courseRepository.GetById(encryptedId));
    }

    [Fact]
    public void GetCoursesByUserId_ShouldReportStatusBasedOnModules()
    {
        var userId = 1;
        var withoutModulesId = _courseRepository.Insert(
            "No modules",
            $"no-modules-{Guid.NewGuid():N}",
            (int)LanguageEnum.Spanish,
            (int)LanguageEnum.English,
            userId);

        var withModulesId = _courseRepository.Insert(
            "With modules",
            $"with-modules-{Guid.NewGuid():N}",
            (int)LanguageEnum.Spanish,
            (int)LanguageEnum.English,
            userId);
        _moduleRepository.Insert(withModulesId, "Flashcards", (int)ModuleTypeEnum.Flashcards, 1, true, userId);

        var courses = _courseRepository.GetCoursesByUserId(userId);
        var withoutModules = courses.First(c => c.Title == "No modules" || _encryptionService.Decrypt(c.Id) == withoutModulesId);
        var withModules = courses.First(c => _encryptionService.Decrypt(c.Id) == withModulesId);

        Assert.False(withoutModules.HasModules);
        Assert.Equal("InProgress", withoutModules.Status);
        Assert.True(withModules.HasModules);
        Assert.Equal("Ready", withModules.Status);
    }
}
