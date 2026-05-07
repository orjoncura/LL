using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace LL.Test.Repositories;

public class WordDifficultyRepositoryTest
{
    private IWordRepository _wordRepository { get; set; }
    private IWordDifficultyRepository _wordDifficultyRepository { get; set; }
    private IEncryptionService _encryptionService { get; set; }
    
    private readonly Mock<ITextToSpeechService> _mockTextToSpeechService;    
    private readonly Mock<IEncryptionService> _mockEncryptionService;

    public WordDifficultyRepositoryTest()
    {
        // Create a mock for TextToSpeechService that doesn't require credentials
        _mockTextToSpeechService = new Mock<ITextToSpeechService>();
        _mockEncryptionService = new Mock<IEncryptionService>();
        
        // Setup the mock to avoid Google Cloud credentials issue
        _mockTextToSpeechService.Setup(x => x.CreateAudio(It.IsAny<string>(), It.IsAny<LanguageEnum>()))
            .Returns(new byte[] { 0x00, 0x01, 0x02 }); // Return mock byte array instead of trying to create real audio

        // Setup the mock EncryptionService
        _mockEncryptionService.Setup(x => x.Encrypt(It.IsAny<int>()))
            .Returns("12345"); // Return a mock encrypted value
        
        _mockEncryptionService.Setup(x => x.Decrypt(It.IsAny<string>()))
            .Returns(12345); // Return a mock decrypted value

        // Create services with the mock
        var services = Provider.GetRequiredService();

        // Override the TextToSpeechService registration with our mock
        services.RemoveAll<ITextToSpeechService>();
        services.AddSingleton<ITextToSpeechService>(_mockTextToSpeechService.Object);    
        
        services.RemoveAll<IEncryptionService>();
        services.AddSingleton<IEncryptionService>(_mockEncryptionService.Object);

        // Build the service provider
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
}
