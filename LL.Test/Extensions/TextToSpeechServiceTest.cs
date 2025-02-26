using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;

namespace LL.Test.Extensions;

public class TextToSpeechServiceTest
{
    private ITextToSpeechService _textToSpeechService { get; set; }
    
    public TextToSpeechServiceTest()
    {
        _textToSpeechService = Provider.GetRequiredService<ITextToSpeechService>();
    }
        
    [Fact]
    public async Task TranslateText_ShouldReturnAStream()
    {
        byte[] results = _textToSpeechService.CreateAudio("a", LanguageEnum.Spanish);
        
        Assert.True(results.Length > 0);
    } 
}