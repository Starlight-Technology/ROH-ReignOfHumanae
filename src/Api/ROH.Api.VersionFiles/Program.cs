//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using FluentValidation;

using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

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

builder.WebHost
    .ConfigureKestrel(
        options =>
        {
            options.ListenAnyIP(
                9100,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                });
            options.ListenAnyIP(
                9200,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            options.Limits.MaxRequestBodySize = null;
        });

// Auto Mapper Configurations
MapperConfiguration mappingConfig = new(
    mc =>
    {
        mc.AddProfile(new GameVersionFileMapping());
        mc.AddProfile(new GameVersionMapping());
        mc.AddProfile(new GameFileMapping());
    },
    NullLoggerFactory.Instance);

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

WebApplication app = builder.Build();

// Apply pending migrations
using (IServiceScope scope = app.Services.CreateScope())
{
    ROH.Context.File.FileContext db = (ROH.Context.File.FileContext)scope.ServiceProvider.GetRequiredService<IFileContext>();
    db.Database.Migrate();
}

static string GetSafeStoredFilePath(ROH.Context.File.Entities.GameFile file)
{
    string rootPath = Path.GetFullPath(file.Path);
    string relativePath = NormalizeRelativePath(file.Name);
    string filePath = Path.GetFullPath(Path.Combine(rootPath, relativePath));
    string rootWithSeparator = rootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
        + Path.DirectorySeparatorChar;

    if (!filePath.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase))
        throw new InvalidOperationException("Invalid file path.");

    return filePath;
}

static string NormalizeRelativePath(string fileName)
{
    string safeName = string.IsNullOrWhiteSpace(fileName) ? "download.bin" : fileName;
    return safeName
        .Replace('\\', Path.DirectorySeparatorChar)
        .Replace('/', Path.DirectorySeparatorChar)
        .TrimStart(Path.DirectorySeparatorChar);
}

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

app.MapGet(
    "DownloadFileRaw",
    async (
        ROH.Context.File.Interface.IGameFileRepository fileRepository,
        ROH.Context.File.Interface.IGameVersionFileRepository versionFileRepository,
        string fileGuid) =>
    {
        if (!Guid.TryParse(fileGuid, out Guid guid))
            return Results.BadRequest();

        var versionFile = await versionFileRepository.GetFileAsync(guid).ConfigureAwait(true);
        var file = versionFile?.GameFile ?? await fileRepository.GetFileAsync(guid).ConfigureAwait(true);
        if (file is null)
            return Results.NotFound();

        string filePath = GetSafeStoredFilePath(file);
        if (!System.IO.File.Exists(filePath))
            return Results.NotFound();

        var stream = System.IO.File.OpenRead(filePath);
        string downloadName = Path.GetFileName(NormalizeRelativePath(file.Name));
        return Results.File(stream, "application/octet-stream", downloadName, lastModified: System.IO.File.GetLastWriteTimeUtc(filePath), entityTag: null, enableRangeProcessing: true);
    })
    .WithName("DownloadFileRaw")
    .WithOpenApi();

app.MapGet(
    "FileChecksum",
    async (
        ROH.Context.File.Interface.IGameFileRepository fileRepository,
        ROH.Context.File.Interface.IGameVersionFileRepository versionFileRepository,
        string fileGuid) =>
    {
        if (!Guid.TryParse(fileGuid, out Guid guid))
            return Results.BadRequest();

        var versionFile = await versionFileRepository.GetFileAsync(guid).ConfigureAwait(true);
        var file = versionFile?.GameFile ?? await fileRepository.GetFileAsync(guid).ConfigureAwait(true);
        if (file is null)
            return Results.NotFound();

        string filePath = GetSafeStoredFilePath(file);
        if (!System.IO.File.Exists(filePath))
            return Results.NotFound();

        using var sha = System.Security.Cryptography.SHA256.Create();
        await using var fs = System.IO.File.OpenRead(filePath);
        byte[] hash = sha.ComputeHash(fs);
        string checksum = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();

        var resp = new ROH.StandardModels.Response.DefaultResponse(objectResponse: checksum);
        return Results.Ok(resp);
    })
    .WithName("FileChecksum")
    .WithOpenApi();

await app.RunAsync().ConfigureAwait(false);
