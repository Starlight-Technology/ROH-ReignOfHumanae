using AutoMapper;

using FluentValidation;

using ROH.Context.PostgreSQLContext;
using ROH.Interfaces;
using ROH.Interfaces.Repository.GameFile;
using ROH.Interfaces.Repository.Log;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Interfaces.Services.GameFile;
using ROH.Interfaces.Services.Version;
using ROH.Mapping.GameFile;
using ROH.Mapping.Version;
using ROH.Repository.GameFile;
using ROH.Repository.Log;
using ROH.Repository.Version;
using ROH.Services.ExceptionService;
using ROH.Services.GameFile;
using ROH.Services.Version;
using ROH.StandardModels.Version;
using ROH.Validations.Version;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registry Interfaces
builder.Services.AddScoped<ISqlContext, SqlContext>();
builder.Services.AddScoped<IGameFileRepository, GameFileRepository>();
builder.Services.AddScoped<IGameVersionFileRepository, GameVersionFileRepository>();
builder.Services.AddScoped<IGameVersionRepository, GameVersionRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();

builder.Services.AddScoped<IGameVersionService, GameVersionService>();
builder.Services.AddScoped<IGameVersionFileService, GameVersionFileService>();
builder.Services.AddScoped<IGameFileService, GameFileService>();

builder.Services.AddScoped<IValidator<GameVersionModel>, GameVersionModelValidator>();
builder.Services.AddScoped<IValidator<GameVersionFileModel>, GameVersionFileModelValidator>();

builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();

// Auto Mapper Configurations
MapperConfiguration mappingConfig = new(mc =>
{
    mc.AddProfile(new GameVersionFileMapping());
    mc.AddProfile(new GameVersionMapping());
    mc.AddProfile(new GameFileMapping());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = null;
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.MapPost("UploadFile", async (IGameVersionFileService _gameVersionFileService, GameVersionFileModel File) =>
    await _gameVersionFileService.NewFile(File).ConfigureAwait(false)
).WithName("UploadFile")
 .WithOpenApi();

app.MapGet("GetAllVersionFiles", async (IGameVersionFileService _gameVersionFileService, string VersionGuid) =>
await _gameVersionFileService.GetFiles(VersionGuid).ConfigureAwait(false)
).WithName("GetAllVersionFiles")
.WithOpenApi();

app.MapGet("DownloadFile", async (IGameVersionFileService _gameVersionFileService, string FileGuid) =>
    await _gameVersionFileService.DownloadFile(new Guid(FileGuid)).ConfigureAwait(false)
).WithName("DownloadFile")
.WithOpenApi();
await app.RunAsync().ConfigureAwait(false);