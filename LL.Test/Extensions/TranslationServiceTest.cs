using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;

namespace LL.Test.Extensions;

public class TranslationServiceTest
{
    private ITranslationService _translationService { get; set; }
    
    public TranslationServiceTest()
    {
        _translationService = Provider.GetRequiredService<ITranslationService>();
    }
        
    [Fact]
    public async Task TranslateText_ShouldTranslateText()
    {        
        Assert.True(_translationService.TranslateText("EN", (int)LanguageEnum.Spanish, (int)LanguageEnum.English).Result.Any(r => r.ToUpper() == "IN"));
        Assert.True(_translationService.TranslateText("HELLO", (int)LanguageEnum.English, (int)LanguageEnum.Spanish).Result.Any(r => r.ToUpper() == "HOLA"));
    } 
}