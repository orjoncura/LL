using LL.Core.Constants;
using LL.Resources.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LL.CA.Configs
{
    public static class DbContextConfig
    {
        public static IServiceCollection AddDbContextConfig(this IServiceCollection services, IConfiguration config)
        {
            string? connectionString = config.GetConnectionString(Secrets.DefaultConnectionString);

            if (connectionString == null) throw new Exception("Connection String is null");

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
