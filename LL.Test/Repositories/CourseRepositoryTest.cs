using LL.Core.Enums;
using LL.Core.Interfaces.Repositories;

namespace LL.Test.Repositories;

public class CourseRepositoryTest
{
    private ICourseRepository _courseRepository { get; set; }

    public CourseRepositoryTest()
    {
        _courseRepository = Provider.GetRequiredService<ICourseRepository>();
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
}
