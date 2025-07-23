#region

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RedFox.Application.Features.Query;
using RedFox.Application.Service.Infrastructure;
using RedFox.Domain.Entities;
using Microsoft.EntityFrameworkCore;


#endregion

namespace RedFox.Application.Features.Handler;

public class DeleteUserHandler(
        IAppDbContext context
    ) : IRequestHandler<DeleteUserCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken ct)
        {
            var entity = await context.Users.FindAsync(new object[] { request.Id }, ct);
            if (entity is null)
                throw new KeyNotFoundException($"Usuario con Id {request.Id} no encontrado.");

            context.Users.Remove(entity);
            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }