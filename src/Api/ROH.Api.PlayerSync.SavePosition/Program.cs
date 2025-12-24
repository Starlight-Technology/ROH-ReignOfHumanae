

using Microsoft.AspNetCore.Server.Kestrel.Core;

using ROH.Context.Player.Mongo;
using ROH.Context.Player.Mongo.Interface;
using ROH.Context.Player.Mongo.Repository;
using ROH.Service.Exception;
using ROH.Service.Exception.Communication;
using ROH.Service.Exception.Interface;
using ROH.Service.Player.Grpc.Player;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();

builder.Services.AddScoped<IPositionRepository, PositionRepository>();

builder.Services.AddScoped<IPlayerMongoContext, PlayerMongoContext>();

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


WebApplication app = builder.Build();

app.MapGrpcService<SavePosition>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Em Program.cs ou Startup.cs da API gRPC
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


await app.RunAsync().ConfigureAwait(true);
