using LL.Core.Helpers;
using LL.Core.Interfaces.Extensions;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;
using LL.Extensions.Models;

namespace LL.Extensions.Services;

public class DictionaryService : IDictionaryService
{
    public async Task<List<MeaningShort>> GetWordDetails(string word)
    {
        try
        {
            // API endpoint for the Dictionary API
            string url = $"https://api.dictionaryapi.dev/api/v2/entries/en/{word}";

            // Make an asynchronous GET request to the API
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            // Check if the response is successful (status code 200)
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            string responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON string into a list of WordData objects (since the API returns an array of results)
            List<WordData> wordDataList = JsonHelper.DeserializeObject<List<WordData>>(responseBody);

            // Return the first entry in the list (assuming you just want the first result)
            return wordDataList?[0].Meanings() ?? new List<MeaningShort>();
        }
        catch (Exception e)
        {
            return new List<MeaningShort>();
        }
    }
}