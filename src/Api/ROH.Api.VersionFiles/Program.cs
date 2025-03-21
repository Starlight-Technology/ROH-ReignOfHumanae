//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using FluentValidation;

using Microsoft.AspNetCore.Server.Kestrel.Core;

using ROH.Context.File;
using ROH.Context.File.Interface;
using ROH.Context.File.Repository;
using ROH.Mapping.GameFile;
using ROH.Mapping.Version;
using ROH.Service.Exception;
using ROH.Service.Exception.Communication;
using ROH.Service.Exception.Interface;
using ROH.Service.File;
using ROH.Service.File.Communication;
using ROH.Service.File.Interface;
using ROH.StandardModels.Version;
using ROH.Validations.Version;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registry Interfaces
builder.Services.AddScoped<IFileContext, FileContext>();
builder.Services.AddScoped<IGameFileRepository, GameFileRepository>();
builder.Services.AddScoped<IGameVersionFileRepository, GameVersionFileRepository>();

builder.Services.AddScoped<IGameVersionService, GameVersionService>();
builder.Services.AddScoped<IGameVersionFileService, GameVersionFileService>();
builder.Services.AddScoped<IGameFileService, GameFileService>();

builder.Services.AddScoped<IValidator<GameVersionModel>, GameVersionModelValidator>();
builder.Services.AddScoped<IValidator<GameVersionFileModel>, GameVersionFileModelValidator>();

builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2; // Supports both protocols
    });
});


// Auto Mapper Configurations
MapperConfiguration mappingConfig = new(
    mc =>
    {
        mc.AddProfile(new GameVersionFileMapping());
        mc.AddProfile(new GameVersionMapping());
        mc.AddProfile(new GameFileMapping());
    });

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = null);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.MapPost(
    "UploadFile",
    async (IGameVersionFileService gameVersionFileService, GameVersionFileModel file) => await gameVersionFileService.NewFileAsync(
        file)
        .ConfigureAwait(false)
)
    .WithName("UploadFile")
    .WithOpenApi();

app.MapGet(
    "GetAllVersionFiles",
    async (IGameVersionFileService gameVersionFileService, string versionGuid) => await gameVersionFileService.GetFilesAsync(
        versionGuid)
        .ConfigureAwait(false)
)
    .WithName("GetAllVersionFiles")
    .WithOpenApi();

app.MapGet(
    "DownloadFile",
    async (IGameVersionFileService gameVersionFileService, string fileGuid) => await gameVersionFileService.DownloadFileAsync(
        new Guid(fileGuid))
        .ConfigureAwait(false)
)
    .WithName("DownloadFile")
    .WithOpenApi();

await app.RunAsync().ConfigureAwait(false);