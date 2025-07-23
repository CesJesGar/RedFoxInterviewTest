#region
using System.Text.Json;
using MediatR;
using RedFox.Application.DTO;
using RedFox.Application.Features.Query;
using RedFox.Application.Service.Api;
using RedFox.Application.Service.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace RedFox.Api.Jobs;

public class DbInitWorker(ILogger<DbInitWorker> logger, IServiceProvider scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (!await TryInitDb(stoppingToken))
            {
                logger.LogInformation("Db already created");
                return;
            }

            await using var userJsonStream = await FetchUsers(stoppingToken);
            var command = await DeserializeJsonStream(userJsonStream, stoppingToken);
            await AddUserToDb(command, stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "An unexpected error occurred during database maintenance.");
        }
    }

    private static async Task<AddUsersWithRelatedCommand> DeserializeJsonStream(
        Stream userJsonStream,
        CancellationToken ct)
    {
        using var jsonDocument = await JsonDocument.ParseAsync(userJsonStream, cancellationToken: ct);

        var userCreationDtos = jsonDocument.RootElement.EnumerateArray()
            .Select(x =>
            {
                var geoEl = x.GetProperty("address").GetProperty("geo");
                var geo = new GeoDto(
                    lat: geoEl.GetProperty("lat").GetString() ?? string.Empty,
                    lng: geoEl.GetProperty("lng").GetString() ?? string.Empty
                );

                var addrEl = x.GetProperty("address");
                var address = new AddressDto(
                    street:  addrEl.GetProperty("street").GetString()  ?? string.Empty,
                    suite:   addrEl.GetProperty("suite").GetString()   ?? string.Empty,
                    city:    addrEl.GetProperty("city").GetString()    ?? string.Empty,
                    zipcode: addrEl.GetProperty("zipcode").GetString() ?? string.Empty,
                    geo:     geo
                );

                var compEl = x.GetProperty("company");
                var company = new CompanyDto(
                    Name:        compEl.GetProperty("name").GetString()       ?? string.Empty,
                    CatchPhrase: compEl.GetProperty("catchPhrase").GetString() ?? string.Empty,
                    Bs:          compEl.GetProperty("bs").GetString()          ?? string.Empty
                );

                return new UserCreationDto(
                    Name:     x.GetProperty("name").GetString()     ?? string.Empty,
                    Username: x.GetProperty("username").GetString() ?? string.Empty,
                    Email:    x.GetProperty("email").GetString()    ?? string.Empty,
                    Phone:    x.GetProperty("phone").GetString()    ?? string.Empty,
                    Website:  x.GetProperty("website").GetString()  ?? string.Empty,
                    Address:  address,
                    Company:  company
                );
            })
            .ToList();

        return new AddUsersWithRelatedCommand(userCreationDtos);
    }

    private async Task AddUserToDb(AddUsersWithRelatedCommand command, CancellationToken ct)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var userIds  = await mediator.Send(command, ct);
        logger.LogInformation("Db init success, added {Count} new users", userIds.Count());
    }

    private async Task<Stream> FetchUsers(CancellationToken ct)
    {
        logger.LogInformation("Fetch initial data");
        await using var scope = scopeFactory.CreateAsyncScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        return await userService.GetUsers(ct);
    }

    private async Task<bool> TryInitDb(CancellationToken ct)
    {
        logger.LogInformation("Running DB Init Worker");
        await using var scope = scopeFactory.CreateAsyncScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
        return await appDbContext.Database.EnsureCreatedAsync(ct);
    }
}