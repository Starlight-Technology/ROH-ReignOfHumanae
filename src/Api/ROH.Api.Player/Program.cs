//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using AutoMapper;

using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging.Abstractions;

using ROH.Context.Player;
using ROH.Context.Player.Interface;
using ROH.Context.Player.Repository;
using ROH.Mapping.Character;
using ROH.Service.Exception;
using ROH.Service.Exception.Communication;
using ROH.Service.Exception.Interface;
using ROH.Service.Player.Characters;
using ROH.Service.Player.Interface;
using ROH.StandardModels.Character;
using ROH.StandardModels.Response;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();

builder.Services.AddScoped<IPlayerContext, PlayerContext>();
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<ICharacterService, CharacterService>();

MapperConfiguration mappingConfig = new(mc => mc.AddProfile(new CharacterMapping()), NullLoggerFactory.Instance);

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.WebHost
    .ConfigureKestrel(
        options => options.ListenAnyIP(
            9105,
            listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2; // Supports both protocols
            }));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.MapPost(
    "CreateCharacter",
    async (ICharacterService service, CharacterModel model) =>
    {
        try
        {
            DefaultResponse result = await service.AddCharacterAsync(model).ConfigureAwait(false);
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, statusCode: 500);
        }
    }
)
    .WithName("CreateCharacter")
    .WithTags("Characters")
    .Accepts<string>("application/json")
    .Produces<string>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status500InternalServerError)
    .WithOpenApi()
    .WithDescription("Create a new character.");

app.MapGet(
    "GetAccountCaracters",
    async (ICharacterService service, Guid accountGuid) => await service.GetAllCharactersAsync(accountGuid)
        .ConfigureAwait(true));

app.MapGet(
    "GetCharacter",
    async (ICharacterService service, Guid characterGuid) => await service.GetCharacterByGuidAsync(characterGuid)
        .ConfigureAwait(true));

await app.RunAsync().ConfigureAwait(false);