using LL.Core.Enums;
using LL.Core.Interfaces.Repositories;

namespace LL.Test.Repositories;

public class ModuleRepositoryTest
{
    private ICourseRepository _courseRepository { get; set; }
    private IModuleRepository _moduleRepository { get; set; }

    public ModuleRepositoryTest()
    {
        _courseRepository = Provider.GetRequiredService<ICourseRepository>();
        _moduleRepository = Provider.GetRequiredService<IModuleRepository>();
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
}
