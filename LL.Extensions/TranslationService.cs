using Google.Cloud.Translation.V2;
using LL.Core.Interfaces.Extensions;

namespace LL.Extensions;

public class TranslationService: ITranslationService
{
    public string TranslateText(string text, int fromId, int toId) => 
        TranslationClient.Create().TranslateText(text, GetLanguageCode(fromId), GetLanguageCode(toId)).TranslatedText;
    
    private string GetLanguageCode(int id)
    {
        string languageCode = string.Empty;
        
        switch(id) 
        {
            case 1:
                languageCode = LanguageCodes.English;
                break;
            case 2:
                languageCode = LanguageCodes.Spanish;
                break;
            default:
                languageCode = LanguageCodes.English;
                break;
        }

        return languageCode;
    }
}