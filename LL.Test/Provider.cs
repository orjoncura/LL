using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Model.DataTransferObjects;
using LL.Core.Models.DataTransferObjects;
using LL.Core.Services;
using LL.Data.Contexts;
using LL.Data.Repositories;
using LL.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LL.Test;

public static class Provider
{
    public static T GetRequiredService<T>() where T : class =>
        GetRequiredService().BuildServiceProvider().GetRequiredService<T>();
    public static ServiceCollection GetRequiredService()
    {
        var services = new ServiceCollection();

        //Core Services
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ISecurityService, SecurityService>();
            
        //Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IWordRepository, WordRepository>();
        services.AddScoped<IWordLinkRepository, WordLinkRepository>();
        services.AddScoped<IWordMeaningRepository, WordMeaningRepository>();
        services.AddScoped<IWordDefinitionRepository, WordDefinitionRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<ICourseWordRepository, CourseWordRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<INewUserRequestRepository, NewUserRequestRepository>();
        services.AddScoped<IResetPasswordRequestRepository, ResetPasswordRequestRepository>();
            
        //Extensions
        services.AddScoped<IAgentService, AgentService>();
        services.AddScoped<IAppMonitoringService, AppMonitoringService>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<ITranslationService, TranslationService>();
        services.AddScoped<ITextToSpeechService, TextToSpeechService>();
        services.AddScoped<IDictionaryService, DictionaryService>();
        services.AddScoped<IEncryptionService, EncryptionService>();

        //Singletons
        services.AddSingleton(new TokenConfigModel(string.Empty,string.Empty,string.Empty,string.Empty));
        services.AddSingleton(new AgentModel(string.Empty, string.Empty));
        services.AddSingleton(new StorageModel(string.Empty, string.Empty, string.Empty));
        
        //Database
        services.AddDbContext<AppDBContext>(options => options.UseInMemoryDatabase("LL_Local"));

        return services;
    }
}