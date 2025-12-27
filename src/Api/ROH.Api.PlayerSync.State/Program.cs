using Microsoft.AspNetCore.Server.Kestrel.Core;

using ROH.Context.Player.Mongo;
using ROH.Context.Player.Mongo.Interface;
using ROH.Context.Player.Mongo.Repository;
using ROH.Service.Exception;
using ROH.Service.Exception.Communication;
using ROH.Service.Exception.Interface;
using ROH.Service.Player.Grpc.Persistence;
using ROH.Service.Player.Grpc.Player;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();

builder.Services.AddScoped<IPositionRepository, PositionRepository>();

builder.Services.AddScoped<IPlayerMongoContext, PlayerMongoContext>();

builder.Services.AddScoped<ROH.Context.Player.Redis.Interface.IPositionRepository, ROH.Context.Player.Redis.Repository.PositionRepository>();

builder.Services.AddScoped<ROH.Context.Player.Redis.Interface.IPlayerRedisContext, ROH.Context.Player.Redis.PlayerRedisContext>();

builder.Services.AddSingleton<IPlayersPersistenceService, PlayersPersistenceService>();

builder.Services.AddGrpc();

builder.WebHost
    .ConfigureKestrel(
        options =>
        {
            options.ListenAnyIP(
                9210,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            options.Limits.MaxRequestBodySize = null;
        });

var app = builder.Build();

app.MapGrpcService<NearbyPlayers>();
app.MapGrpcService<SavePosition>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


await app.RunAsync().ConfigureAwait(true);

