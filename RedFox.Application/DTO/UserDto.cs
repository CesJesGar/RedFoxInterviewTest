namespace RedFox.Application.DTO;

public record UserDto(
    int Id,
    string Name,
    string Username,
    string Email,
    string Phone,
    string Website,
    AddressDto Address,
    CompanyDto Company);