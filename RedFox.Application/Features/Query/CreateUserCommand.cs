namespace RedFox.Application.Features.Query
{
    using MediatR;
    using RedFox.Application.DTO;

    public record CreateUserCommand(UserCreationDto User) : IRequest<UserDto>;
}
