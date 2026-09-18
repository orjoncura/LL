using LL.Core.Constants;
using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace LL.Test.Repositories;

public class ModuleRepositoryTest
{
    private readonly ICourseRepository _courseRepository;
    private readonly IModuleRepository _moduleRepository;
    private readonly IEncryptionService _encryptionService;

    public ModuleRepositoryTest()
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
    public void Insert_ShouldCreateModule()
    {
        var courseId = _courseRepository.Insert(
            "Module course",
            $"module-value-{Guid.NewGuid():N}",
            (int)LanguageEnum.English,
            (int)LanguageEnum.Spanish,
            1);

        var moduleId = _moduleRepository.Insert(
            courseId,
            "Flashcards",
            (int)ModuleTypeEnum.Flashcards,
            1,
            true,
            1);

        Assert.True(moduleId > 0);
    }

    [Fact]
    public void ModuleCreated_ShouldReturnTrueAfterInsert()
    {
        var courseId = _courseRepository.Insert(
            "Created check",
            $"module-created-{Guid.NewGuid():N}",
            (int)LanguageEnum.Spanish,
            (int)LanguageEnum.English,
            1);

        Assert.False(_moduleRepository.ModuleCreated(courseId, (int)ModuleTypeEnum.Flashcards));

        _moduleRepository.Insert(courseId, "Flashcards", (int)ModuleTypeEnum.Flashcards, 1, true, 1);

        Assert.True(_moduleRepository.ModuleCreated(courseId, (int)ModuleTypeEnum.Flashcards));
    }

    [Fact]
    public void GetByCourseId_ShouldReturnModules()
    {
        var courseId = _courseRepository.Insert(
            "Get modules",
            $"get-modules-{Guid.NewGuid():N}",
            (int)LanguageEnum.Spanish,
            (int)LanguageEnum.English,
            1);
        _moduleRepository.Insert(courseId, "Flashcards", (int)ModuleTypeEnum.Flashcards, 1, true, 1);

        var modules = _moduleRepository.GetByCourseId(_encryptionService.Encrypt(courseId));

        Assert.NotEmpty(modules);
    }

    [Fact]
    public void MarkModuleAsComplete_ShouldMarkCompletedAndUnlockNext()
    {
        var courseId = _courseRepository.Insert(
            "Complete module",
            $"complete-module-{Guid.NewGuid():N}",
            (int)LanguageEnum.Spanish,
            (int)LanguageEnum.English,
            1);
        var firstId = _moduleRepository.Insert(courseId, "First", (int)ModuleTypeEnum.Flashcards, 1, true, 1);
        var secondId = _moduleRepository.Insert(courseId, "Second", (int)ModuleTypeEnum.Exercises, 2, false, 1);

        Assert.True(_moduleRepository.MarkModuleAsComplete(_encryptionService.Encrypt(firstId)));

        var modules = _moduleRepository.GetByCourseId(_encryptionService.Encrypt(courseId));
        Assert.Contains(modules, m => m.Title == "First" && m.Completed);
        Assert.Contains(modules, m => m.Title == "Second" && m.Unlocked);
    }
}
