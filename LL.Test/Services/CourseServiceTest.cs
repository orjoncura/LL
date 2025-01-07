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
    public class CourseServiceTest
    {
        private ICourseService SeminarService { get; set; }
        public CourseServiceTest()
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
                new()
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

            SeminarService = services.BuildServiceProvider().GetRequiredService<ICourseService>();
        }
        
        [Fact]
        public async Task CreateCourse_ShouldCreateCourse()
        {            
            CourseRequestModel courseRequest = new CourseRequestModel()
            {
                LanguageFromId = (int)LanguageEnum.Spanish,
                LanguageToId = (int)LanguageEnum.English,
                Text = "Creo en los milagros desde que te vi"
            };

            // Act
            CourseViewModel seminars = await SeminarService.CreateCourse(courseRequest, 1);

            // Assert
            Assert.True(seminars.IsValid);
        } 
          
    }   
}