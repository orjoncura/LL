using LL.API.Configs;
using LL.SharedDefinitions.Static;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddUserSecrets<Program>();

        Secret.Configuration = builder.Configuration;

        // Add services to the container.
        builder.Services
            .AddDbContextConfig(builder)
            .AddCorsConfig()
            .AddDIConfig()
            .AddJwtConfig()
            .AddSwaggerGen()
            .AddEndpointsApiExplorer()
            .AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowSpecificOrigin");
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}