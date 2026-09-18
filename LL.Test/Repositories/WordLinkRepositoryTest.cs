using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Resources.Contexts;
using LL.Resources.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace LL.Test.Repositories;

public class WordLinkRepositoryTest
{
    private IWordRepository _wordRepository { get; set; }
    private IWordLinkRepository _wordLinkRepository { get; set; }
    private AppDbContext _dbContext { get; set; }

    private readonly Mock<ITextToSpeechService> _mockTextToSpeechService;

    public WordLinkRepositoryTest()
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
        _wordLinkRepository = serviceProvider.GetRequiredService<IWordLinkRepository>();
        _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
    }

    [Fact]
    public void Insert_ShouldCreateWordLink()
    {
        var wordId = _wordRepository.Insert($"new-link-{Guid.NewGuid():N}", (int)LanguageEnum.Spanish, 1).Id;

        var result = _wordLinkRepository.Insert(
            wordId,
            "hello",
            (int)LanguageEnum.Spanish,
            (int)LanguageEnum.English,
            1);

        Assert.True(result.Id > 0);
        Assert.Equal("hello", result.Translation);
    }

    [Fact]
    public void Insert_ShouldReturnExistingWordLink()
    {
        var wordId = _wordRepository.Insert($"link-{Guid.NewGuid():N}", (int)LanguageEnum.English, 1).Id;

        var existing = new WordLink
        {
            WordId = wordId,
            Value = "hola",
            LanguageId = (int)LanguageEnum.Spanish,
            IsActive = true,
            CreatedById = 1,
            CreatedDate = DateTime.Now
        };
        _dbContext.WordLinks.Add(existing);
        _dbContext.SaveChanges();

        var result = _wordLinkRepository.Insert(
            wordId,
            "hola",
            (int)LanguageEnum.English,
            (int)LanguageEnum.Spanish,
            1);

        Assert.Equal(existing.Id, result.Id);
    }
}

