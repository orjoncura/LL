using LL.Core.Constants;
using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace LL.Test.Repositories;

public class WordDifficultyRepositoryTest
{
    private readonly IWordRepository _wordRepository;
    private readonly IWordDifficultyRepository _wordDifficultyRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly Mock<ITextToSpeechService> _mockTextToSpeechService;

    public WordDifficultyRepositoryTest()
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
        _wordDifficultyRepository = serviceProvider.GetRequiredService<IWordDifficultyRepository>();
        _encryptionService = serviceProvider.GetRequiredService<IEncryptionService>();
    }

    [Fact]
    public void SetWordDifficulty_ShouldReturnTrue()
    {
        var wordId = _wordRepository.Insert($"difficulty-{Guid.NewGuid():N}", (int)LanguageEnum.English, 1).Id;
        var encryptedWordId = _encryptionService.Encrypt(wordId);

        var result = _wordDifficultyRepository.SetWordDifficulty((int)ImportanceRatingEnum.High, encryptedWordId, 1);

        Assert.True(result);
    }

    [Fact]
    public void GetWordCountByDifficulty_ShouldCountUserWords()
    {
        var wordId = _wordRepository.Insert($"count-{Guid.NewGuid():N}", (int)LanguageEnum.Spanish, 1).Id;
        var encryptedWordId = _encryptionService.Encrypt(wordId);
        const int userId = 1;

        _wordDifficultyRepository.SetWordDifficulty((int)ImportanceRatingEnum.Low, encryptedWordId, userId);

        var count = _wordDifficultyRepository.GetWordCountByDifficulty(
            (int)ImportanceRatingEnum.Low,
            (int)LanguageEnum.Spanish,
            userId);

        Assert.True(count >= 1);
    }

    [Fact]
    public void GetWordsByDifficulty_ShouldReturnWords()
    {
        var wordId = _wordRepository.Insert($"list-{Guid.NewGuid():N}", (int)LanguageEnum.Spanish, 1).Id;
        var encryptedWordId = _encryptionService.Encrypt(wordId);
        const int userId = 1;

        _wordDifficultyRepository.SetWordDifficulty((int)ImportanceRatingEnum.Medium, encryptedWordId, userId);

        var words = _wordDifficultyRepository.GetWordsByDifficulty(
            (int)ImportanceRatingEnum.Medium,
            (int)LanguageEnum.Spanish,
            userId);

        Assert.NotNull(words);
    }
}
