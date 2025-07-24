#region 
using MediatR;
using RedFox.Api.Jobs;
using RedFox.Application;
using RedFox.Application.Features.Query;
using RedFox.Infrastructure;
using RedFox.Application.DTO;
using RedFox.Application.Features.Query;
using FluentValidation;
using FluentValidation.AspNetCore;
using RedFox.Application.Validators;
#endregion

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<UserCreationDtoValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
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
    
    async (UserCreationDto dto, IValidator<UserCreationDto> validator, IMediator mediator, CancellationToken ct) =>
    {
        var result = await validator.ValidateAsync(dto, ct);
        if (!result.IsValid)
            return Results.BadRequest(result.Errors);

        var command = new CreateUserCommand(dto);

        
        var created = await mediator.Send(command, ct);

        
        return Results.Created($"/users/{created.Id}", created);
    })
.WithName("CreateUser")
.WithOpenApi();

// PUT /users/{id}
app.MapPut("/users/{id:int}", async (int id, UserUpdateDto dto, IValidator<UserUpdateDto> validator, IMediator mediator, CancellationToken ct) =>
{
    var result = await validator.ValidateAsync(dto, ct);
        if (!result.IsValid)
            return Results.BadRequest(result.Errors);

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