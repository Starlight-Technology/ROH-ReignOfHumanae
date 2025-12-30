//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MessagePack;
using MessagePack.Resolvers;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using ROH.Service.Exception;
using ROH.Service.Exception.Communication;
using ROH.Service.Exception.Interface;
using ROH.Service.Player.WebSocket.Interface;
using ROH.Service.Player.WebSocket.State;
using ROH.Service.WebSocket;

using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

MessagePackSerializer.DefaultOptions =
    MessagePackSerializerOptions.Standard
    .WithResolver(StandardResolver.Instance)
    .WithSecurity(MessagePackSecurity.UntrustedData);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddGrpc();

string tokenKey = Environment.GetEnvironmentVariable("ROH_KEY_TOKEN") ?? "thisisaverysecurekeywith32charslong!";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        options => options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                ValidIssuer = "ROH.Services.Authentication.AuthService",
                ValidAudience = "ROH.Gateway"
            });

builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddSwaggerGen(
        c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ROH-Gateway", Version = "v1" });

            c.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });
        });

// Configure Kestrel to listen on a specific port
builder.WebHost
    .ConfigureKestrel(
        options =>
        {
            options.Limits.MaxRequestBodySize = null;
            options.ListenAnyIP(
                9001,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                });
            options.ListenAnyIP(
                9002,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
        });

builder.Services.AddSingleton<ILogService, LogService>();

builder.Services.AddSingleton<IExceptionHandler, ExceptionHandler>();

builder.Services
    .AddSingleton<ROH.Gateway.Controllers.Websocket.IRealtimeConnectionManager, ROH.Gateway.Controllers.Websocket.RealtimeConnectionManager>(
        );

builder.Services.AddSingleton<IPlayerPositionServiceSocket, PlayerPositionServiceSocket>();

WebApplication app = builder.Build();

app.UseWebSockets(new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(15) });

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<PlayersConnected>();

app.MapControllers();
app.Map("/ws", RealtimeWebSocketEndpoint);

await app.RunAsync().ConfigureAwait(true);

static async Task RealtimeWebSocketEndpoint(HttpContext context)
{
    if (!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = 400;
        return;
    }

    System.Net.WebSockets.WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();

    ROH.Gateway.Controllers.Websocket.IRealtimeConnectionManager manager = context.RequestServices
        .GetRequiredService<ROH.Gateway.Controllers.Websocket.IRealtimeConnectionManager>();

    await manager.HandleClientAsync(context, socket);
}