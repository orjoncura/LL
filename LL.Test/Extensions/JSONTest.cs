using LL.SharedDefinitions.Models;
using LL.Extensions.Models;
using LL.Extensions;
using LL.Core.Factories;
using LL.Data.Model;
using LL.Core.Enums;
using LL.Core.Models;

namespace LL.Test.Extensions
{
    public class JSONTest
    {
        [Fact]
        public void JSON_ExtractSeminarViewModel()
        {
            SeminarViewModel? seminarViewModel = new SeminarViewModel();

            string jsonString = PromptFactory.CreateSeminarPrompt("Creo", (int)LanguageEnum.English, (int)LanguageEnum.Spanish);

            seminarViewModel = JSON.Extract<SeminarViewModel>(jsonString);

            Assert.True(seminarViewModel?.Sentences.Any());
        }
        
        [Fact]
        public void JSON_ExtractSeminarWordsModel()
        {
            List<SeminarWordsModel> seminarWordsModel = new List<SeminarWordsModel>();

            string jsonString = PromptFactory.CreateSeminarWordsPrompt(string.Empty, (int)LanguageEnum.Spanish);

            seminarWordsModel = JSON.Extract<List<SeminarWordsModel>>(jsonString);

            Assert.True(seminarWordsModel?.Any());
        }
    }   
}