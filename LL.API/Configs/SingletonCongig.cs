using LL.Core.Models.ViewModel;

namespace LL.API.Configs;

public static class SingletonCongig
{
    public static IServiceCollection AddSingletonCongig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new AgentModel(configuration["GeminiAPI"], configuration["LLamaModeLocation"]));
        
        return services;
    }
}