using LL.Core.Services;
using LL.Data.Interfaces;
using LL.SharedDefinitions.Model;
using LL.Core.Enums;
using Moq;

namespace LL.Test.Services
{
    public class SeminarServiceTest()
    {
        [Fact]
        public async Task CreateSeminar_ShouldCreateSeminar()
        {
            // Arrange
            var wordRepository = new Mock<IWordRepository>();
            var statementRepository = new Mock<IStatementRepository>();
            var service = new SeminarService(wordRepository.Object, statementRepository.Object);

            SeminarRequestModel seminarRequest = new SeminarRequestModel(){
                LanguageIdFrom = (int)LanguageEnum.English,
                LangaugeIdTo = (int)LanguageEnum.Spanish,
                Words = new List<string>(){ "Creo", "en", "los", "milagros", "desde", "que" }
            };

            // Act
            List<SeminarViewModel> seminars = await service.CreateSeminar(seminarRequest, 1);

            // Assert
            Assert.True(seminars.All(s => s.IsValid));
        }
    }   
}