using LL.Core.Constants;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Models.ViewModel;
using LL.Core.Services;
using LL.Data.Contexts;
using LL.Data.Repositories;
using LL.Extensions;
using LL.Test.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LL.Test;

public static class Provider
{
    public static T GetRequiredService<T>()
    {
        var services = new ServiceCollection();
            
        var config = new ConfigurationBuilder().AddUserSecrets<SeminarServiceTest>().Build();
            
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
            
        //Extensions
        services.AddScoped<IAgentService>(p => 
            new AgentService(new AgentModel(config[Secrets.GeminiAPI], config[Secrets.LLamaLocation])));
            
        services.AddScoped<IAppMonitoringService, AppMonitoringService>();
        services.AddScoped<IAgentService, AgentService>();
        services.AddScoped<ITranslationService, TranslationService>();
        services.AddScoped<IDictionaryService, DictionaryService>();
            
        //Database
        services.AddDbContext<AppDBContext>(options => options.UseInMemoryDatabase("LL_Local"));
            
        return services.BuildServiceProvider().GetRequiredService<T>();
    }
}