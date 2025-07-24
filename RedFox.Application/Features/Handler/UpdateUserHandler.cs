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


 /// <summary>
    /// Inyecta context y mapper para operaciones con EF Core y AutoMapper.
    /// </summary>
namespace RedFox.Application.Features.Handler;

public class UpdateUserHandler(
        IAppDbContext context,
        IMapper mapper
    ) : IRequestHandler<UpdateUserCommand, UserDto>
    {
        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken ct)
        {
            var entity = await context.Users
                .Include(u => u.Company)
                .Include(u => u.Address)
                    .ThenInclude(a => a.Geo)
                .FirstOrDefaultAsync(u => u.Id == request.Id, ct);

            if (entity is null)
                throw new KeyNotFoundException($"Usuario con Id {request.Id} no encontrado.");

            mapper.Map(request.User, entity); // ← actualiza todo correctamente

            await context.SaveChangesAsync(ct);
            return mapper.Map<UserDto>(entity);

            await context.SaveChangesAsync(ct);
            return mapper.Map<UserDto>(entity);
        }
    }