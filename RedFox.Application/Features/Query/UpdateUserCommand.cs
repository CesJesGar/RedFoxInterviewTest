#region
using MediatR;
using RedFox.Application.DTO;
#endregion

namespace RedFox.Application.Features.Query
{
    public record UpdateUserCommand(int Id, UserUpdateDto User) : IRequest<UserDto>;
}
