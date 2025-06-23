namespace LL.API.Configs
{
    public static class CorsConfig
    {
        public static IServiceCollection AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policy => policy.WithOrigins("http://localhost:3000", "https://web.fluente.dynv6.net")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });

            return services;
        }
    }
}
