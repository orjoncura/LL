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
            
        //Services
        services.AddScoped<ISeminarService, SeminarService>();
            
        //Repositories
        services.AddScoped<ISeminarRepository, SeminarRepository>();
        services.AddScoped<IWordRepository, WordRepository>();
        services.AddScoped<IWordLinkRepository, WordLinkRepository>();
        services.AddScoped<IWordMeaningRepository, WordMeaningRepository>();
        services.AddScoped<IWordDefinitionRepository, WordDefinitionRepository>();
        services.AddScoped<IStatementRepository, StatementRepository>();
        services.AddScoped<ISeminarWordRepository, SeminarWordRepository>();
            
        //Extensions
        services.AddScoped<IAppMonitoringService, AppMonitoringService>();
        services.AddScoped<IAgentService, AgentService>();
        services.AddScoped<ITranslationService, TranslationService>();
        services.AddScoped<IDictionaryService, DictionaryService>();

        //Singletons
        services.AddSingleton(new AgentModel(config["GeminiAPI"], config["LLamaModeLocation"]));
            
        //Database
        services.AddDbContext<AppDBContext>(options => options.UseInMemoryDatabase("LL_Local"));
            
        return services.BuildServiceProvider().GetRequiredService<T>();
    }
}