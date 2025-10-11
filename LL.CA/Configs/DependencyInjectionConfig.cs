using LL.Core.Constants;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Model.DataTransferObjects;
using LL.Core.Services;
using LL.Resources.Repositories;
using LL.Resources.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LL.CA.Configs
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjectionConfig(this IServiceCollection services, IConfiguration config)
        {
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
            services.AddScoped<ITextToSpeechService, TextToSpeechService>();
            services.AddScoped<IEncryptionService, EncryptionService>();

            //Singletons
            services.AddSingleton(
                new TokenConfigModel(
                    config[Secrets.JwtKey],
                config[Secrets.JwtIssuer],
                config[Secrets.JwtAudience],
                config[Secrets.JwtExpiryMinutes]));

            services.AddSingleton(new AgentModel(config[Secrets.GeminiAPI], config[Secrets.LLamaLocation]));
             
            return services;
        }
    }
}
