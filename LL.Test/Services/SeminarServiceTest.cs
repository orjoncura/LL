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
                LanguageToId = (int)LanguageEnum.Spanish,
                Text = "Creo en los milagros desde que te vi\nEn esta noche de tequila boom boom" +
                       "Eres tan sexy eres sexy thing\nMis ojos te persiguen sólo a ti\n\nY debe haber un caos dentro de ti" +
                       "Para que brote así una estrella que baila\nInfierno y paraíso dentro de ti\nLa luna es un sol, mira cómo brilla" +
                       "Baby the night is on fire\nSeamos fuego en el cielo\nLlamas en lo oscuro\n\nWhat you say\n\nBaila baila morena" +
                       "Bajo esta luna llena\nUnder the moonlight\nUnder the moonlight\n\nVen chica ven loca dame tu boca" +
                       "Que en esta noche cualquier cosa te toca\nMi corazón de oro es el de un santo\nDámelo todo me lo merezco tanto"
            };

            // Act
            List<SeminarViewModel> seminars = await service.CreateSeminar(seminarRequest, 1);

            // Assert
            Assert.True(seminars.All(s => s.IsValid));
        } 
          
    }   
}