using AutoMapper;
using RedFox.Application.DTO;
using RedFox.Domain.Entities;

namespace RedFox.Application.Profiles;

/// <summary>
/// Configura los mapeos entre entidades de dominio y DTOs,
/// ignorando propiedades generadas (Id) y resolviendo colecciones/nombres distintos.
/// </summary>

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Geo <-> GeoDto: mapea todas las propiedades excepto el Id
        CreateMap<Geo, GeoDto>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // Address <-> AddressDto: mapea la propiedad Geo y omite Id
        CreateMap<Address, AddressDto>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Geo, opt => opt.MapFrom(src => src.geo));
        // Company <-> CompanyDto: omitimos el Id en el mapeo inverso
        CreateMap<Company, CompanyDto>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        // User -> UserDto: mapeo directo de entidad a DTO de lectura
        CreateMap<User, UserDto>();
        // Mapea UserCreationDto -> User al crear
        CreateMap<UserCreationDto, User>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
         // Mapea UserUpdateDto -> User al actualizar (ignora Id para no reescribir PK)
        CreateMap<UserUpdateDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
    }
}