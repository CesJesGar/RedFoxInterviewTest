using AutoMapper;
using RedFox.Application.DTO;
using RedFox.Domain.Entities;

namespace RedFox.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Geo, GeoDto>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<Address, AddressDto>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Geo, opt => opt.MapFrom(src => src.geo));

        CreateMap<Company, CompanyDto>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<User, UserDto>();

        CreateMap<UserCreationDto, User>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

        CreateMap<UserUpdateDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
    }
}