using LL.SharedDefinitions.Models;
using LL.Extensions.Models;
using LL.Extensions;
using LL.Core.Factories;
using LL.Data.Model;
using LL.Core.Enums;

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
    }   
}