#region

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RedFox.Application.Features.Query;
using RedFox.Application.Service.Infrastructure;
using RedFox.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using RedFox.Application.DTO;


#endregion

namespace RedFox.Application.Features.Handler;

public class CreateUserHandler(
        IAppDbContext context,
        IMapper mapper
    ) : IRequestHandler<CreateUserCommand, UserDto>
    {
        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken ct)
        {
            var entity = mapper.Map<User>(request.User);

            context.Users.Add(entity);
            await context.SaveChangesAsync(ct);

            return mapper.Map<UserDto>(entity);
        }
    }