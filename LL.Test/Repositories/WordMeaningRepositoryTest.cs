using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace LL.Test.Repositories;

public class WordMeaningRepositoryTest
{
    private IWordRepository _wordRepository { get; set; }
    private IWordMeaningRepository _wordMeaningRepository { get; set; }
    private readonly Mock<ITextToSpeechService> _mockTextToSpeechService;

    public WordMeaningRepositoryTest()
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
        _wordMeaningRepository = serviceProvider.GetRequiredService<IWordMeaningRepository>();
    }

    [Fact]
    public void Insert_ShouldCreateWordMeaning()
    {
        var wordId = _wordRepository.Insert($"meaning-{Guid.NewGuid():N}", (int)LanguageEnum.English, 1).Id;
        var wordMeaningId = _wordMeaningRepository.Insert(wordId, (int)WordTypeEnum.Noun, 1);

        Assert.True(wordMeaningId > 0);
    }

    [Fact]
    public void GetByWordId_ShouldReturnMeanings()
    {
        var wordId = _wordRepository.Insert($"meaning-get-{Guid.NewGuid():N}", (int)LanguageEnum.English, 1).Id;
        _wordMeaningRepository.Insert(wordId, (int)WordTypeEnum.Verb, 1);

        var meanings = _wordMeaningRepository.GetByWordId(wordId);

        Assert.NotNull(meanings);
        Assert.NotEmpty(meanings!);
    }
}
