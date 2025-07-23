using AutoMapper;
using RedFox.Application.DTO;
using RedFox.Domain.Entities;

namespace RedFox.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Geo, GeoDto>().ReverseMap();
        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<Company, CompanyDto>().ReverseMap();
        CreateMap<User, UserDto>();
        CreateMap<UserCreationDto, User>()
            .ForMember(dest => dest.Company,
                       opt => opt.MapFrom(src => src.Company))
            .ForMember(dest => dest.Address,
                       opt => opt.MapFrom(src => src.Address));

        CreateMap<UserUpdateDto, User>()
            .ForMember(dest => dest.Company,
                       opt => opt.MapFrom(src => src.Company))
            .ForMember(dest => dest.Address,
                       opt => opt.MapFrom(src => src.Address));
    }
}