#region
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RedFox.Application.DTO;
using RedFox.Application.Features.Query;       // AddUsersWithRelatedCommand
using RedFox.Application.Service.Api;
using RedFox.Application.Service.Infrastructure;
#endregion

namespace RedFox.Api.Jobs;

/// <summary>
/// Servicio en background que crea la base de datos y la siembra
/// con los 10 usuarios de JSONPlaceholder (incluyendo Address/Geo/Company).
/// </summary>

public class DbInitWorker : BackgroundService
{
    private readonly ILogger<DbInitWorker> _logger;
    private readonly IServiceProvider      _scopeFactory;

    public DbInitWorker(ILogger<DbInitWorker> logger, IServiceProvider scopeFactory)
    {
        _logger       = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            // 1) Intentamos crear la DB; si ya existe, abortamos.
            if (!await TryInitDb(stoppingToken))
            {
                _logger.LogInformation("Db already created");
                return;
            }
             // 1.1) Deserializamos a DTOs y mandamos comando de inserción
            await using var userJsonStream = await FetchUsers(stoppingToken);
            var command = await DeserializeJsonStream(userJsonStream, stoppingToken);
            await AddUserToDb(command, stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "An unexpected error occurred during database maintenance.");
        }
    }

   private async Task<AddUsersWithRelatedCommand> DeserializeJsonStream(
    Stream userJsonStream,
    CancellationToken ct)
{
    using var jsonDocument = await JsonDocument.ParseAsync(userJsonStream, cancellationToken: ct);
    var root = jsonDocument.RootElement;

    var creationDtos = new List<UserCreationDto>();

    foreach (var x in root.EnumerateArray())
    {
        // 2) Loggear el JSON crudo de este elemento
        _logger.LogInformation("Procesando elemento JSON: {Json}", x.GetRawText());

        // 3) Ahora extrae geo, address, company y crea el DTO
        var geoEl = x.GetProperty("address").GetProperty("geo");
        var geo = new GeoDto(
            geoEl.GetProperty("lat").GetString() ?? string.Empty,
            geoEl.GetProperty("lng").GetString() ?? string.Empty
        );

        var addrEl = x.GetProperty("address");
        var address = new AddressDto(
            addrEl.GetProperty("street").GetString()  ?? string.Empty,
            addrEl.GetProperty("suite").GetString()   ?? string.Empty,
            addrEl.GetProperty("city").GetString()    ?? string.Empty,
            addrEl.GetProperty("zipcode").GetString() ?? string.Empty,
            geo
        );

        var compEl = x.GetProperty("company");
        var company = new CompanyDto(
            compEl.GetProperty("name").GetString()       ?? string.Empty,
            compEl.GetProperty("catchPhrase").GetString()?? string.Empty,
            compEl.GetProperty("bs").GetString()          ?? string.Empty
        );

        var dto = new UserCreationDto(
            x.GetProperty("name").GetString()     ?? string.Empty,
            x.GetProperty("username").GetString() ?? string.Empty,
            x.GetProperty("email").GetString()    ?? string.Empty,
            x.GetProperty("phone").GetString()    ?? string.Empty,
            x.GetProperty("website").GetString()  ?? string.Empty,
            address,
            company
        );

        creationDtos.Add(dto);
    }

    return new AddUsersWithRelatedCommand(creationDtos);
}


    private async Task AddUserToDb(AddUsersWithRelatedCommand command, CancellationToken ct)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var userIds  = await mediator.Send(command, ct);
        _logger.LogInformation("Db init success, added {Count} new users", userIds.Count());
    }

    private async Task<Stream> FetchUsers(CancellationToken ct)
    {
        _logger.LogInformation("Fetch initial data");
        await using var scope = _scopeFactory.CreateAsyncScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        return await userService.GetUsers(ct);
    }

    private async Task<bool> TryInitDb(CancellationToken ct)
    {
        _logger.LogInformation("Running DB Init Worker");
        await using var scope = _scopeFactory.CreateAsyncScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
        return await appDbContext.Database.EnsureCreatedAsync(ct);
    }
}
