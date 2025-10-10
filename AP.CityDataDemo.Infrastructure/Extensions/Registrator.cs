using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Infrastructure.Contexts;
using AP.CityDataDemo.Infrastructure.Repositories;
using AP.CityDataDemo.Infrastructure.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Infrastructure.Services;

namespace AP.CityDataDemo.Infrastructure.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDbContext(configuration);
            services.RegisterRepositories();
            services.RegisterServices();
            return services;
        }

        private static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CityDataDemoContext>(options =>
            {
                options.UseSqlServer("Server=sqlserver,1433;Database=CityDataDemo;User ID=sa;Password=YourStrong@Passw0rd;Encrypt=False;TrustServerCertificate=True;")
                       .EnableSensitiveDataLogging();
            });
            return services;
        }

        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }
    }
}