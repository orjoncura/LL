using System;
using Google.Cloud.Translation.V2;

namespace LL.Extensions;

public static class Translation
{
    public static string TranslateText(string text, int fromId, int toId) => 
        TranslationClient.Create().TranslateText(text, GetLanguageCode(fromId), GetLanguageCode(toId)).TranslatedText;
    
    private static string GetLanguageCode(int id)
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