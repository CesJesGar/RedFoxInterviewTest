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

public class AddUsersWithRelatedCommandHandler(
    IAppDbContext context,
    IMapper mapper,
    ILogger<AddUsersWithRelatedCommandHandler> logger
) : IRequestHandler<AddUsersWithRelatedCommand, IEnumerable<int>>
{
    public async Task<IEnumerable<int>> Handle(AddUsersWithRelatedCommand request, CancellationToken ct)
    {
        await using var tx = await context.Database.BeginTransactionAsync(ct);
         try
        {
            var users = request.Users
                .Select(dto => mapper.Map<User>(dto))
                .ToList();                         

            await context.Users.AddRangeAsync(users, ct);
            await context.SaveChangesAsync(ct);

            await tx.CommitAsync(ct);
            return users.Select(u => u.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Batch user creation failed");
            await tx.RollbackAsync(ct);
            throw;
        }
    }
}