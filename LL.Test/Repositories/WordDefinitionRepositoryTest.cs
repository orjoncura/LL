using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Model.DataTransferObjects;
using LL.Core.Services;
using LL.Resources.Contexts;
using LL.Resources.Repositories;
using LL.Resources.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Xunit;

namespace LL.Test.Repositories;

public class WordDefinitionRepositoryTest
{
    private IWordRepository _wordRepository { get; set; }
    private IWordMeaningRepository _wordMeaningRepository { get; set; }
    private IWordDefinitionRepository _wordDefinitionRepository { get; set; }

    private readonly Mock<ITextToSpeechService> _mockTextToSpeechService;
    
    public WordDefinitionRepositoryTest()
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
        _wordDefinitionRepository = serviceProvider.GetRequiredService<IWordDefinitionRepository>();
    }

    [Fact]
    public void Insert_ShouldCreateWordDefinition()
    {
        var wordId = _wordRepository.Insert($"definition-{Guid.NewGuid():N}", (int)LanguageEnum.English, 1).Id;
        var wordMeaningId = _wordMeaningRepository.Insert(wordId, (int)WordTypeEnum.Noun, 1);
        var definitionId = _wordDefinitionRepository.Insert("A sample definition", wordMeaningId, 1);

        Assert.True(definitionId > 0);
    }
}
