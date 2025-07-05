using Application.Features.Cities.Dtos;
using AutoMapper;

namespace Application.Common.Mapping
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityDto>();
        }
    }
}
