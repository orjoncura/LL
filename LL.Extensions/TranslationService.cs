using System.Net.Http.Headers;
using System.Web;
using LL.Core.Helpers;
using LL.Core.Interfaces.Extensions;
using LL.Extensions.Models;

namespace LL.Extensions;

public class TranslationService: ITranslationService
{
    public async Task<string> TranslateText(string text, int fromId, int toId)
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.GetAsync($"https://655.mtis.workers.dev/translate?" +
                                             $"text={HttpUtility.UrlEncode(text)}" +
                                             $"&source_lang={GetLanguageCode(fromId)}" +
                                             $"&target_lang={GetLanguageCode(toId)}");
        response.EnsureSuccessStatusCode();
        
        // Deserialize with case-insensitive handling
        TranslationModel translationModel = 
            JsonHelper.DeserializeObject<TranslationModel>(await response.Content.ReadAsStringAsync()) 
            ?? new TranslationModel();
        
        return  translationModel.Response.TranslatedText;
    }

    private string GetLanguageCode(int id)
    {
        string languageCode = "en";
        
        if(id == 2)
            languageCode = "es";

        return languageCode;
    }
}