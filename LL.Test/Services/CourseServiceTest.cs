using LL.Core.Constants;
using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace LL.Test.Services;

public class CourseServiceTest
{   
    private readonly ICourseService _courseService;
    private readonly ICourseRepository _courseRepository;

    public CourseServiceTest()
    { 
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c["Course:ServiceUrl"]).Returns("https://api.example.com");
        mockConfiguration.Setup(c => c["Course:ApiKey"]).Returns("test-api-key");
        mockConfiguration.Setup(c => c[Secrets.EncryptionKey])
            .Returns("edTWS52cRCrRB4NDDCwT6mY6dMcWwa3n");
        
        var services = Provider.GetRequiredService();
        services.RemoveAll<IConfiguration>();
        services.AddSingleton(mockConfiguration.Object);
        
        var mockAgentService = new Mock<IAgentService>();
        mockAgentService.Setup(service => service.Run(It.IsAny<string>()))
            .ReturnsAsync("[{\"Word\":\"Creo\",\"Translation\":\"I believe\",\"Definition\":\"to think\",\"PartOfSpeech\":\"Verb\",\"Importance\":1}]");
        
        var mockTextToSpeechService = new Mock<ITextToSpeechService>();
        mockTextToSpeechService.Setup(service => service.CreateAudio(It.IsAny<string>(), It.IsAny<LanguageEnum>()))
            .Returns([]);
        
        var mockTranscriptionService = new Mock<ITranscriptionService>();
        mockTranscriptionService.Setup(s => s.TranscribeFromUrl(It.IsAny<string>()))
            .ReturnsAsync("Creo en los milagros desde que te vi");

        services.RemoveAll<IAgentService>();
        services.RemoveAll<ITextToSpeechService>();
        services.RemoveAll<ITranscriptionService>();
        services.AddTransient(_ => mockAgentService.Object);
        services.AddTransient(_ => mockTextToSpeechService.Object);
        services.AddTransient(_ => mockTranscriptionService.Object);

        var serviceProvider = services.BuildServiceProvider();
        _courseService = serviceProvider.GetRequiredService<ICourseService>();
        _courseRepository = serviceProvider.GetRequiredService<ICourseRepository>();
    }
    
    [Fact]
    public async Task CreateCourse_ShouldCreateCourse()
    {            
        var courseRequest = new CourseRequestModel()
        {
            Title = $"Course-{Guid.NewGuid():N}",
            LanguageFromId = (int)LanguageEnum.Spanish,
            LanguageToId = (int)LanguageEnum.English,
            Text = "Creo en los milagros desde que te vi"
        };
        
        Assert.True(await _courseService.CreateCourse(courseRequest, 1));
    }

    [Fact]
    public async Task CreateCourse_WithInvalidRequest_ShouldReturnFalse()
    {
        var courseRequest = new CourseRequestModel
        {
            Title = "Invalid",
            LanguageFromId = 0,
            LanguageToId = 0
        };

        Assert.False(await _courseService.CreateCourse(courseRequest, 1));
    }

    [Fact]
    public async Task CreateCourse_ShouldAppearInUserCoursesImmediatelyAsInProgressOrReady()
    {
        var title = $"Immediate-{Guid.NewGuid():N}";
        var courseRequest = new CourseRequestModel()
        {
            Title = title,
            LanguageFromId = (int)LanguageEnum.Spanish,
            LanguageToId = (int)LanguageEnum.English,
            Text = "Hola amigo como estas hoy"
        };

        var createTask = _courseService.CreateCourse(courseRequest, 1);

        // Give the service a moment to insert the pending course row.
        await Task.Delay(50);
        var coursesDuringCreate = _courseRepository.GetCoursesByUserId(1);
        Assert.Contains(coursesDuringCreate, c => c.Title == title);

        Assert.True(await createTask);

        var coursesAfterCreate = _courseRepository.GetCoursesByUserId(1);
        var created = coursesAfterCreate.First(c => c.Title == title);
        Assert.True(created.HasModules || created.Status is "Ready" or "InProgress" or "Failed");
    }
}
