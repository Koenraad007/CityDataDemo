using System.Reflection;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AP.CityDataDemo.Application.Extensions
{
    public static class Registrator
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection services)
        {
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}