using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Infrastructure.Contexts;
using AP.CityDataDemo.Infrastructure.Repositories;
using AP.CityDataDemo.Infrastructure.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AP.CityDataDemo.Infrastructure.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
        {
            services.RegisterDbContext();
            services.RegisterRepositories();
            return services;
        }

        private static IServiceCollection RegisterDbContext(this IServiceCollection services)
        {
            services.AddDbContext<CityDataDemoContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "CityDataDemoDB");
            });
            return services;
        }

        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICitiesRepository, CitiesRepository>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}