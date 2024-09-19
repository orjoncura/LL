using LL.API.Configs;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddUserSecrets<Program>();
        
        // Add services to the container.
        builder.Services
            .AddDbContextConfig(builder.Configuration)
            //.AddJwtConfig(builder.Configuration)
            .AddCorsConfig()
            .AddDependencyInjectionConfig()
            .AddSingletonCongig(builder.Configuration)
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