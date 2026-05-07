using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace LL.Test.Repositories;

public class CourseWordRepositoryTest
{
    private IWordRepository _wordRepository { get; set; }
    private ICourseRepository _courseRepository { get; set; }
    private IModuleRepository _moduleRepository { get; set; }
    private ICourseWordRepository _courseWordRepository { get; set; }
    private readonly Mock<ITextToSpeechService> _mockTextToSpeechService;

    public CourseWordRepositoryTest()
    {
        // Create a mock for TextToSpeechService that doesn't require credentials
        _mockTextToSpeechService = new Mock<ITextToSpeechService>();

        // Setup the mock to avoid Google Cloud credentials issue
        _mockTextToSpeechService.Setup(x => x.CreateAudio(It.IsAny<string>(), It.IsAny<LanguageEnum>()))
            .Returns(new byte[] { 0x00, 0x01, 0x02 }); // Return mock byte array instead of trying to create real audio

        // Create services with the mock
        var services = Provider.GetRequiredService();

        // Override the TextToSpeechService registration with our mock
        services.RemoveAll<ITextToSpeechService>();
        services.AddSingleton<ITextToSpeechService>(_mockTextToSpeechService.Object);

        // Build the service provider
        var serviceProvider = services.BuildServiceProvider();
        _wordRepository = serviceProvider.GetRequiredService<IWordRepository>();
        _courseRepository = serviceProvider.GetRequiredService<ICourseRepository>();
        _moduleRepository = serviceProvider.GetRequiredService<IModuleRepository>();
        _courseWordRepository = serviceProvider.GetRequiredService<ICourseWordRepository>();
    }
    
    [Fact]
    public void Insert_ShouldCreateCourseWord()
    {
        var wordId = _wordRepository.Insert($"word-{Guid.NewGuid():N}", (int)LanguageEnum.English, 1).Id;
        var courseId = _courseRepository.Insert(
            "Course",
            $"course-{Guid.NewGuid():N}",
            (int)LanguageEnum.English,
            (int)LanguageEnum.Spanish,
            1);
        var moduleId = _moduleRepository.Insert(courseId, "Module", (int)ModuleTypeEnum.Flashcards, 1, true, 1);

        var courseWordId = _courseWordRepository.Insert(wordId, moduleId, (int)ImportanceRatingEnum.Medium, 1);

        Assert.True(courseWordId > 0);
    }
}
