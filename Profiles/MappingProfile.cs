using AutoMapper;
using HotelApi.Data;
using HotelApi.DTOS.ReadDtos;
using HotelApi.DTOS.WriteDtos;

namespace HotelApi.Profiles
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, CountryReadDto>().ReverseMap();
            CreateMap<CountryWriteDto, Country>().ReverseMap();

            CreateMap<Hotel, HotelReadDto>().ReverseMap();
            CreateMap<HotelWriteDto, HotelReadDto>();

            CreateMap<UserWriteDto, ApiUser>().ReverseMap();

            CreateMap<LoginWriteDto, ApiUser>().ReverseMap();
        }
    }
}
