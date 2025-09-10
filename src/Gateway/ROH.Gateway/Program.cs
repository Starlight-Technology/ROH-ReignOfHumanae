//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using ROH.Gateway.Grpc.Player;
using ROH.Protos.NearbyPlayer;
using ROH.Protos.PlayerPosition;
using ROH.Utils.ApiConfiguration;

using System.Collections.Generic;
using System.Text;

using static ROH.Protos.PlayerPosition.PlayerService;
using static ROH.Utils.ApiConfiguration.ApiConfigReader;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

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

            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
                });
        });

// Configure Kestrel to listen on a specific port
builder.WebHost
    .ConfigureKestrel(
        options =>
        {
            options.Limits.MaxRequestBodySize = null;
            options.ListenAnyIP
                (
                    9001,
                    listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1; // Supports both protocols
                    }
                );
            options.ListenAnyIP
                (
                    9002,
                    listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    }
                );
        });

if (!Api._apiUrl.TryGetValue(ApiUrl.PlayerSavePosition, out var savePosUri))
    throw new InvalidOperationException("URL para PlayerSavePosition não encontrada na configuração.");
builder.Services.AddGrpcClient<PlayerService.PlayerServiceClient>(options =>
{
    options.Address = savePosUri;
});

if (!Api._apiUrl.TryGetValue(ApiUrl.GetNearbyPlayer, out var nearbyUri))
    throw new InvalidOperationException("URL para NearbyPlayerGrpc não encontrada na configuração.");
builder.Services.AddGrpcClient<NearbyPlayerService.NearbyPlayerServiceClient>(options =>
{
    options.Address = nearbyUri;
});

builder.Services.AddGrpc();

WebApplication app = builder.Build();

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

app.MapGrpcService<PlayerSavePositionServiceForwarder>().EnableGrpcWeb().AllowAnonymous();
app.MapGrpcService<NearbyPlayerServiceForwarder>().EnableGrpcWeb().AllowAnonymous();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync().ConfigureAwait(true);