using LL.Core.Constants;
using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace LL.Test.Repositories;

public class WordRepositoryTest
{
    private readonly IWordRepository _wordRepository;
    private readonly IWordLinkRepository _wordLinkRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IModuleRepository _moduleRepository;
    private readonly ICourseWordRepository _courseWordRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly Mock<ITextToSpeechService> _mockTextToSpeechService;

    public WordRepositoryTest()
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
        _wordLinkRepository = serviceProvider.GetRequiredService<IWordLinkRepository>();
        _courseRepository = serviceProvider.GetRequiredService<ICourseRepository>();
        _moduleRepository = serviceProvider.GetRequiredService<IModuleRepository>();
        _courseWordRepository = serviceProvider.GetRequiredService<ICourseWordRepository>();
        _encryptionService = serviceProvider.GetRequiredService<IEncryptionService>();
    }

    [Fact]
    public void Insert_ShouldCreateWord()
    {
        Assert.True(_wordRepository.Insert($"hola-{Guid.NewGuid():N}", (int)LanguageEnum.Spanish, 1).Id > 0);
    }

    [Fact]
    public void GetRangeByText_ShouldReturnMatchingWords()
    {
        var name = $"range-{Guid.NewGuid():N}";
        var word = _wordRepository.Insert(name, (int)LanguageEnum.Spanish, 1);
        _wordLinkRepository.Insert(word.Id, "translation", (int)LanguageEnum.Spanish, (int)LanguageEnum.English, 1);

        var results = _wordRepository.GetRangeByText(new List<string> { name }, (int)LanguageEnum.Spanish, 1);

        Assert.Contains(results, w => w.Name == name);
    }

    [Fact]
    public void GetKeyWords_ShouldReturnHighOrMediumImportanceWords()
    {
        var name = $"keyword-{Guid.NewGuid():N}";
        var word = _wordRepository.Insert(name, (int)LanguageEnum.Spanish, 1, (int)ImportanceRatingEnum.High);
        _wordLinkRepository.Insert(word.Id, "key translation", (int)LanguageEnum.Spanish, (int)LanguageEnum.English, 1);

        var results = _wordRepository.GetKeyWords((int)LanguageEnum.Spanish);

        Assert.Contains(results, w => w.Name == name);
    }

    [Fact]
    public void GetByModuleId_ShouldReturnCourseWords()
    {
        var name = $"module-word-{Guid.NewGuid():N}";
        var word = _wordRepository.Insert(name, (int)LanguageEnum.Spanish, 1);
        _wordLinkRepository.Insert(word.Id, "module translation", (int)LanguageEnum.Spanish, (int)LanguageEnum.English, 1);

        var courseId = _courseRepository.Insert(
            "Word course",
            $"word-course-{Guid.NewGuid():N}",
            (int)LanguageEnum.Spanish,
            (int)LanguageEnum.English,
            1);
        var moduleId = _moduleRepository.Insert(courseId, "Flashcards", (int)ModuleTypeEnum.Flashcards, 1, true, 1);
        _courseWordRepository.Insert(word.Id, moduleId, (int)ImportanceRatingEnum.Medium, 1);

        var results = _wordRepository.GetByModuleId(_encryptionService.Encrypt(moduleId));

        Assert.Contains(results, w => w.Name == name);
    }

    [Fact]
    public void GetFileStreamById_ShouldReturnStream()
    {
        var word = _wordRepository.Insert($"audio-{Guid.NewGuid():N}", (int)LanguageEnum.Spanish, 1);
        var stream = _wordRepository.GetFileStreamById(_encryptionService.Encrypt(word.Id));

        Assert.NotNull(stream);
        Assert.True(stream!.Length > 0);
    }
}
