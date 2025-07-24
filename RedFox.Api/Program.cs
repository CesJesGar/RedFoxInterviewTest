#region 
using MediatR;
using RedFox.Api.Jobs;
using RedFox.Application;
using RedFox.Application.Features.Query;
using RedFox.Infrastructure;
using RedFox.Application.DTO;
using RedFox.Application.Features.Query;
#endregion

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<DbInitWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 4. Endpoints CRUD usando MediatR

// GET /users

// app.MapGet("/users", async (IMediator context) => await context.Send(new GetAllUserWithRelatedQuery()))
//     .WithName("GetUsers")
//     .WithOpenApi();

app.MapGet("/users", async (IMediator mediator, CancellationToken ct) =>
{
    var users = await mediator.Send(new GetAllUserWithRelatedQuery(), ct);
    return Results.Ok(users);
})
.WithName("GetUsers")
.WithOpenApi();

// GET /users/{id}
app.MapGet("/users/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
{
    var user = await mediator.Send(new GetUserWithRelatedQuery(id), ct);
    return user is null
        ? Results.NotFound()
        : Results.Ok(user);
})
.WithName("GetUserById")
.WithOpenApi();

// POST /users
app.MapPost("/users",
    // ① Recibe directamente el JSON como UserCreationDto
    async (UserCreationDto dto, IMediator mediator, CancellationToken ct) =>
    {
        // ② Envuelve el DTO en tu comando
        var command = new CreateUserCommand(dto);

        // ③ Manda el comando y obtiene el UserDto de respuesta
        var created = await mediator.Send(command, ct);

        // ④ Devuelve 201 Created con la URI y el nuevo objeto
        return Results.Created($"/users/{created.Id}", created);
    })
.WithName("CreateUser")
.WithOpenApi();

// PUT /users/{id}
// PUT /users/{id}
app.MapPut("/users/{id:int}", async (int id, UserUpdateDto dto, IMediator mediator, CancellationToken ct) =>
{
    var command = new UpdateUserCommand(id, dto);

    var updated = await mediator.Send(command, ct);
    return Results.Ok(updated);
})
.WithName("UpdateUser")
.WithOpenApi();

// DELETE /users/{id}
app.MapDelete("/users/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
{
    await mediator.Send(new DeleteUserCommand(id), ct);
    return Results.NoContent();
})
.WithName("DeleteUser")
.WithOpenApi();

await app.RunAsync();