namespace RedFox.Application.DTO;
public record AddressDto(
    string street,
    string suite,
    string city,
    string zipcode,
    GeoDto geo
);
