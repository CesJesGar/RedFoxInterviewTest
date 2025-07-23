#region

using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using RedFox.Application.Service.Api;

#endregion

namespace RedFox.Infrastructure.Api;

public class UserService(HttpClient httpClient, ILogger<UserService> logger) : IUserService
{
    private const string BasePath = "users";

    public async Task<Stream> GetUsers(CancellationToken ct)
    {
        try
        {
            var res = await httpClient.GetAsync(BasePath, ct);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStreamAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetUsers falló");
            throw;
        }
    }

    public async Task<Stream> GetUserById(int id, CancellationToken ct)
    {
        try
        {
            var res = await httpClient.GetAsync($"{BasePath}/{id}", ct);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStreamAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetUserById falló para ID {UserId}", id);
            throw;
        }
    }

    public async Task<Stream> CreateUser(Stream userJsonStream, CancellationToken ct)
    {
        try
        {
            using var content = new StreamContent(userJsonStream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var res = await httpClient.PostAsync(BasePath, content, ct);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStreamAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CreateUser falló");
            throw;
        }
    }

    public async Task<Stream> UpdateUser(int id, Stream userJsonStream, CancellationToken ct)
    {
        try
        {
            using var content = new StreamContent(userJsonStream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var res = await httpClient.PutAsync($"{BasePath}/{id}", content, ct);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStreamAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UpdateUser falló para ID {UserId}", id);
            throw;
        }
    }

    public async Task DeleteUser(int id, CancellationToken ct)
    {
        try
        {
            var res = await httpClient.DeleteAsync($"{BasePath}/{id}", ct);
            res.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "DeleteUser falló para ID {UserId}", id);
            throw;
        }
    }
}
