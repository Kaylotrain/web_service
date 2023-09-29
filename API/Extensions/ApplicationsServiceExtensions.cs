
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using web_service.Data;

namespace API.Extensions
{
    public static class ApplicationsServiceExtensions
    {
        public static IServiceCollection AddApplicationsServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}