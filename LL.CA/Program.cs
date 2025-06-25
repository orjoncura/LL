using LL.CA.Configs;
using LL.Core.Enums;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LL.CA;

public class Program
{
    private static ICourseService courseService { get; set; }
    
    public static async Task Main(string[] args)
    {
        try
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            
            // Add user secrets from Program.cs
            builder.Configuration.AddUserSecrets<Program>();
            
            builder.Services
                .AddDbContextConfig(builder.Configuration)
                .AddDependencyInjectionConfig(builder.Configuration);
            
            // Add logging
            builder.Services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
            });
            
            courseService = builder.Services.BuildServiceProvider().GetRequiredService<ICourseService>();
            //await courseService.CreateCourse(courseRequestModel, -1);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
