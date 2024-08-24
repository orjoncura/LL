using LL.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LL.API.Configs
{
    public static class DbContextConfig
    {
        public static IServiceCollection AddDbContextConfig(this IServiceCollection services, string? connectionString)
        {
            if(connectionString == null) throw new Exception("Connection String is null");

            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
