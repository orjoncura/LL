using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using LL.Core.Models.DataTransferObjects;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;
using LL.Core.Services;
using LL.Extensions.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace LL.Test.Services
{
    public class SeminarServiceTest
    {
        private ISeminarService _seminarService { get; set; }
        public SeminarServiceTest()
        { 
            var services = Provider.GetRequiredService();
            
            var mockAgentService = new Mock<IAgentService>();
            var expectedResult = "[{\"Word\":\"Creo\",\"Importance\":1}]";
            mockAgentService.Setup(service => service.Run(It.IsAny<string>())).ReturnsAsync(expectedResult);
            
            var mockTextToSpeechService = new Mock<ITextToSpeechService>();
            mockTextToSpeechService.Setup(service => service.CreateAudio(It.IsAny<string>(), LanguageEnum.English)).Returns([]);
            
            var mockStorageService = new Mock<IStorageService>();
            mockStorageService.Setup(service => service.SaveFile(It.IsAny<StorageModel>(), It.IsAny<byte[]>())).ReturnsAsync(string.Empty);
            
            var mockDictionaryService = new Mock<IDictionaryService>();
            List<MeaningShort> meaningShorts = new List<MeaningShort>()
            {
                new MeaningShort()
                {
                    Type = "Type",
                    Definitions = new List<string>()
                    {
                        "Test", "Test"
                    }
                }
            };
            mockDictionaryService.Setup(service => service.GetWordDetails(It.IsAny<string>())).ReturnsAsync(meaningShorts);
            
            services.AddTransient<IAgentService>(_ => mockAgentService.Object);
            services.AddTransient<ITextToSpeechService>(_ => mockTextToSpeechService.Object);
            services.AddTransient<IStorageService>(_ => mockStorageService.Object);
            services.AddTransient<IDictionaryService>(_ => mockDictionaryService.Object);

            _seminarService = services.BuildServiceProvider().GetRequiredService<ISeminarService>();
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
            SeminarViewModel seminars = await _seminarService.CreateSeminar(seminarRequest, 1);

            // Assert
            Assert.True(seminars.IsValid);
        } 
          
    }   
}