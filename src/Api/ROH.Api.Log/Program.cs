//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//     Author:  
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Server.Kestrel.Core;

using ROH.Api.Log.Services;
using ROH.Context.Log;
using ROH.Context.Log.Interface;
using ROH.Context.Log.Repository;
using ROH.Service.Log;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Registry Interfaces

builder.Services.AddScoped<ILogContext, LogContext>();

builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddGrpc();

builder.WebHost
    .ConfigureKestrel(
        options =>
        {
            options.ListenAnyIP(
                9104,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                });
            options.ListenAnyIP(
                9204,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
        });

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGrpcService<LogServiceImplementation>();

await app.RunAsync().ConfigureAwait(true);
