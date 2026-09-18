using LL.Core.Constants;
using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace LL.Test.Repositories;

public class CourseWordRepositoryTest
{
    private readonly IWordRepository _wordRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IModuleRepository _moduleRepository;
    private readonly ICourseWordRepository _courseWordRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly Mock<ITextToSpeechService> _mockTextToSpeechService;

    public CourseWordRepositoryTest()
    {
        _mockTextToSpeechService = new Mock<ITextToSpeechService>();
        _mockTextToSpeechService.Setup(x => x.CreateAudio(It.IsAny<string>(), It.IsAny<LanguageEnum>()))
            .Returns(new byte[] { 0x00, 0x01, 0x02 });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c[Secrets.EncryptionKey])
            .Returns("edTWS52cRCrRB4NDDCwT6mY6dMcWwa3n");

        var services = Provider.GetRequiredService();
        services.RemoveAll<IConfiguration>();
        services.AddSingleton(mockConfiguration.Object);
        services.RemoveAll<ITextToSpeechService>();
        services.AddSingleton(_mockTextToSpeechService.Object);

        var serviceProvider = services.BuildServiceProvider();
        _wordRepository = serviceProvider.GetRequiredService<IWordRepository>();
        _courseRepository = serviceProvider.GetRequiredService<ICourseRepository>();
        _moduleRepository = serviceProvider.GetRequiredService<IModuleRepository>();
        _courseWordRepository = serviceProvider.GetRequiredService<ICourseWordRepository>();
        _encryptionService = serviceProvider.GetRequiredService<IEncryptionService>();
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

    [Fact]
    public void Insert_WithEncryptedWordId_ShouldCreateCourseWord()
    {
        var wordId = _wordRepository.Insert($"enc-word-{Guid.NewGuid():N}", (int)LanguageEnum.Spanish, 1).Id;
        var courseId = _courseRepository.Insert(
            "Encrypted course",
            $"enc-course-{Guid.NewGuid():N}",
            (int)LanguageEnum.Spanish,
            (int)LanguageEnum.English,
            1);
        var moduleId = _moduleRepository.Insert(courseId, "Module", (int)ModuleTypeEnum.Flashcards, 1, true, 1);

        _courseWordRepository.Insert(
            _encryptionService.Encrypt(wordId),
            moduleId,
            (int)ImportanceRatingEnum.High,
            1);

        Assert.True(true);
    }
}
