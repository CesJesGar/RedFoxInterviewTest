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

public class GetUserWithRelatedHandler(IAppDbContext context, IMapper mapper) :
    IRequestHandler<GetUserWithRelatedQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserWithRelatedQuery request, CancellationToken ct)
{
     var user = await context.Users
                .Include(u => u.Company)
                .Include(u => u.Address)
                    .ThenInclude(a => a.Geo)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

            return mapper.Map<UserDto>(user);
}
}