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
                LanguageFromId = (int)LanguageEnum.Spanish,
                LanguageToId = (int)LanguageEnum.English,
                Text = "Creo en los milagros desde que te vi"
            };

            // Act
            List<SeminarViewModel> seminars = await _seminarService.CreateSeminar(seminarRequest, 1);

            // Assert
            Assert.True(seminars.All(s => s.IsValid));
        } 
          
    }   
}