//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Server.Kestrel.Core;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddGrpc();

builder.WebHost
    .ConfigureKestrel(
        options =>
        {
            options.ListenAnyIP(
                9211,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            options.Limits.MaxRequestBodySize = null;
        });

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

await app.RunAsync().ConfigureAwait(true);