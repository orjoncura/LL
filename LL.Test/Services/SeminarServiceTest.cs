using LL.Core.Enums;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;

namespace LL.Test.Services
{
    public class SeminarServiceTest
    {
        private ISeminarService _seminarService { get; set; }
        public SeminarServiceTest()
        {
            _seminarService = Provider.GetRequiredService<ISeminarService>();
        }
        
        [Fact]
        public async Task CreateSeminar_ShouldCreateSeminar()
        {           
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
            List<SeminarViewModel> seminars = await _seminarService.CreateSeminar(seminarRequest, 1);

            // Assert
            Assert.True(seminars.All(s => s.IsValid));
        } 
          
    }   
}