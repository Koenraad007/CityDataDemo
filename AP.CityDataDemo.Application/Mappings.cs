using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Domain;
using AutoMapper;

namespace AP.CityDataDemo.Application
{
    internal class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<City, CityDto>()
            .ForMember(cdto => cdto.CountryName, opt => opt.MapFrom(c => c.Country != null ? c.Country.Name : "N/A"));
            CreateMap<City, CityDto>().ReverseMap();

            CreateMap<Country, CountryDto>();
            CreateMap<Country, CountryDto>().ReverseMap();
        }
    }
}