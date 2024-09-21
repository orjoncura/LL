using LL.Core.Factories;
using LL.Core.Enums;
using LL.Core.Helpers;
using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;

namespace LL.Test.Extensions
{
    public class JSONTest
    {
        [Fact]
        public void JSON_ExtractSeminarViewModel()
        {
            SeminarViewModel? seminarViewModel = new SeminarViewModel();

            string jsonString = PromptFactory.CreateSeminarPrompt("Creo", (int)LanguageEnum.English, (int)LanguageEnum.Spanish);

            seminarViewModel = JsonHelper.Extract<SeminarViewModel>(jsonString);

            Assert.True(seminarViewModel?.Sentences.Any());
        }
        
        [Fact]
        public void JSON_ExtractSeminarWordsModel()
        {
            List<SeminarWordsModel> seminarWordsModel = new List<SeminarWordsModel>();

            string jsonString = PromptFactory.CreateSeminarWordsPrompt(string.Empty, (int)LanguageEnum.Spanish);

            seminarWordsModel = JsonHelper.Extract<List<SeminarWordsModel>>(jsonString);

            Assert.True(seminarWordsModel?.Any());
        }
    }   
}