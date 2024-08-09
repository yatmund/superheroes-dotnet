using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Superheroes.Configuration;
using Superheroes.ExceptionHandlers;
using Superheroes.Providers;
using Superheroes.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ICharactersProvider>()
    .AddStandardResilienceHandler();
builder.Services.AddSingleton<ICharactersProvider, CharactersProvider>();
builder.Services.AddScoped<IBattleService, BattleService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var settings = builder.Configuration.GetSection(nameof(CharactersSettings));
builder.Services.Configure<CharactersSettings>(settings);

var app = builder.Build();

app.MapGet("/battle", async ([AsParameters] BattleQuery battle, IBattleService service) =>
{
    battle.Validate();
    var result = await service.BattleAsync(battle.Hero, battle.Villain);

    return result.IsFailure
        ? Results.BadRequest(result.Error)
        : Results.Ok(result.Payload);
});


app.UseExceptionHandler();
app.Run();

public partial class Program {}
