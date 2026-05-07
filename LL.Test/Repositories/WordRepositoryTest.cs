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
    private IWordRepository _wordRepository { get; set; }
    private readonly Mock<ITextToSpeechService> _mockTextToSpeechService;
    
    public WordRepositoryTest()
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
    }

    [Fact]
    public async Task Insert_AddRecords()
    {
        // Assert
        Assert.True(_wordRepository.Insert("Hola", (int)LanguageEnum.Spanish, 1).Id > 0);
    } 
}