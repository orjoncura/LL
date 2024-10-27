using Google.Apis.Requests;
using LL.Core.Constants;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Models.ViewModel;
using LL.Core.Services;
using LL.Data.Contexts;
using LL.Data.Repositories;
using LL.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LL.Test;

public static class Provider
{
    public static IConfiguration GetConfiguration<T>() where T : class =>
        new ConfigurationBuilder().AddUserSecrets<T>().Build();

    
    public static T GetRequiredService<T>() where T : class
    {
        var services = new ServiceCollection();
            
        var config = GetConfiguration<T>();
        
        //Core Services
        services.AddScoped<ISeminarService, SeminarService>();
        services.AddScoped<ISecurityService, SecurityService>();
            
        //Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISeminarRepository, SeminarRepository>();
        services.AddScoped<IWordRepository, WordRepository>();
        services.AddScoped<IWordLinkRepository, WordLinkRepository>();
        services.AddScoped<IWordMeaningRepository, WordMeaningRepository>();
        services.AddScoped<IWordDefinitionRepository, WordDefinitionRepository>();
        services.AddScoped<IStatementRepository, StatementRepository>();
        services.AddScoped<ISeminarWordRepository, SeminarWordRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<INewUserRequestRepository, NewUserRequestRepository>();
        services.AddScoped<IResetPasswordRequestRepository, ResetPasswordRequestRepository>();
            
        //Extensions
        services.AddSingleton(new AgentModel(config[Secrets.GeminiAPI], config[Secrets.LLamaLocation]));
        services.AddScoped<IAgentService, AgentService>();
        services.AddScoped<IAppMonitoringService, AppMonitoringService>();
        services.AddScoped<ITranslationService, TranslationService>();
        services.AddScoped<IDictionaryService, DictionaryService>();
        
        services.AddSingleton(new TokenConfigModel(config[Secrets.JwtKey], config[Secrets.JwtIssuer], config[Secrets.JwtAudience]));
        
        //Database
        services.AddDbContext<AppDBContext>(options => options.UseInMemoryDatabase("LL_Local"));

        return services.BuildServiceProvider().GetRequiredService<T>();
    }
}