using LL.Core.Services;
using LL.Data.Contexts;
using LL.Data.Interfaces;
using LL.SharedDefinitions.Model;
using Microsoft.EntityFrameworkCore;
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

            SeminarRequestModel seminarRequest = new SeminarRequestModel();

            // Act
            List<SeminarViewModel> seminars = await service.CreateSeminar(seminarRequest, 1);

            // Assert
            Assert.True(seminars.Where(s => string.IsNullOrWhiteSpace(s.TargetWord.OriginalStatement) == false
                && string.IsNullOrWhiteSpace(s.TargetWord.TranslatedStatement) == false).Any());

        }
    }
}