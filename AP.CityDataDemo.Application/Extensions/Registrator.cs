using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AP.CityDataDemo.Application.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICountryService, CountryService>();
            return services;
        }
    }
}