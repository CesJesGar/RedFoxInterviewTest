namespace RedFox.Application.Features.Query
{
    using MediatR;
    public record DeleteUserCommand(int Id) : IRequest<Unit>;
}
