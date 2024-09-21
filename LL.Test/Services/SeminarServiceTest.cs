using LL.Core.Services;
using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;
using Moq;
using Microsoft.Extensions.Configuration;

namespace LL.Test.Services
{
    public class SeminarServiceTest
    {    
        private Mock<ISeminarRepository> _seminarRepository { get; set; }
        private Mock<IWordRepository> _wordRepository { get; set; }
        private Mock<IWordLinkRepository> _wordLinkRepository { get; set; }
        private Mock<IStatementRepository> _statementRepository { get; set; }
        private Mock<ISeminarWordRepository> _seminarWordRepository { get; set; }
        private Mock<IAgentService> _agentService { get; set; }
        private Mock<ITranslationService> _translation { get; set; }
        
        public SeminarServiceTest()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<SeminarServiceTest>().Build();
            
            _seminarRepository = new Mock<ISeminarRepository>();
            _wordRepository = new Mock<IWordRepository>();
            _wordLinkRepository = new Mock<IWordLinkRepository>();
            _statementRepository = new Mock<IStatementRepository>();
            _seminarWordRepository = new Mock<ISeminarWordRepository>();
            _agentService = new Mock<IAgentService>();
            _translation = new Mock<ITranslationService>();
        }

        [Fact]
        public async Task CreateSeminar_ShouldCreateSeminar()
        {
            var seminarService = new SeminarService(
                _seminarRepository.Object,
                _wordRepository.Object,
                _wordLinkRepository.Object,
                _statementRepository.Object,
                _seminarWordRepository.Object,
                _agentService.Object,
                _translation.Object);

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
            List<SeminarViewModel> seminars = await seminarService.CreateSeminar(seminarRequest, 1);

            // Assert
            Assert.True(seminars.All(s => s.IsValid));
        } 
          
    }   
}