using LL.Core.Services;
using LL.Data.Interfaces;
using LL.SharedDefinitions.Models;
using LL.Core.Enums;
using Moq;
using LL.SharedDefinitions.Static;
using Microsoft.Extensions.Configuration;

namespace LL.Test.Services
{
    public class SeminarServiceTest
    {
        public SeminarServiceTest()
        {
            Secret.Configuration = new ConfigurationBuilder().AddUserSecrets<SeminarServiceTest>().Build();
        }

        [Fact]
        public async Task CreateSeminar_ShouldCreateSeminar()
        {
            // Arrange
            var wordRepository = new Mock<IWordRepository>();
            var statementRepository = new Mock<IStatementRepository>();

            var service = new SeminarService(wordRepository.Object, statementRepository.Object);

            SeminarRequestModel seminarRequest = new SeminarRequestModel()
            {
                LanguageFromId = (int)LanguageEnum.English,
                LangaugeToId = (int)LanguageEnum.Spanish,
                Words = new List<string>(){ "Creo", "en", "los", "milagros", "desde", "que" }
            };

            // Act
            List<SeminarViewModel> seminars = await service.CreateSeminar(seminarRequest, 1);

            // Assert
            Assert.True(seminars.All(s => s.IsValid));
        } 
          
    }   
}