using AutoMapper;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

using ROH.Context.Version;
using ROH.Context.Version.Interface;
using ROH.Context.Version.Repository;
using ROH.Mapping.GameFile;
using ROH.Mapping.Version;
using ROH.Service.Exception;
using ROH.Service.Exception.Interface;
using ROH.Service.Version;
using ROH.Service.Version.Interface;
using ROH.StandardModels.Version;
using ROH.Utils.Helpers;
using ROH.Validations.Version;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registry Interfaces
builder.Services.AddScoped<IVersionContext, VersionContext>();
builder.Services.AddScoped<IGameVersionRepository, GameVersionRepository>();

builder.Services.AddScoped<IGameVersionService, GameVersionService>();

builder.Services.AddScoped<IValidator<GameVersionModel>, GameVersionModelValidator>();

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

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.MapPost("CreateNewVersion", async (IGameVersionService gameVersionService, GameVersionModel model) =>
    await gameVersionService.NewVersionAsync(model).ConfigureAwait(false)
).WithName("CreateNewVersion")
.WithOpenApi();

app.MapPut("ReleaseVersion", async (IGameVersionService gameVersionService, [FromBody] GameVersionModel gameVersion) =>
    await gameVersionService.SetReleasedAsync(gameVersion.Guid.ToString()).ConfigureAwait(false)
).WithName("ReleaseVersion")
.WithOpenApi();

app.MapGet("GetCurrentVersion", (IGameVersionService gameVersionService) =>
    gameVersionService.GetCurrentVersionAsync().Result.MapObjectResponse<GameVersionModel>()
).WithName("GetCurrentVersion")
.WithOpenApi();

app.MapGet("GetAllVersionsPaginated", async (IGameVersionService gameVersionService, int page, int take) =>
    await gameVersionService.GetAllVersionsAsync(page: page, take: take).ConfigureAwait(false)
).WithName("GetAllVersionsPaginated")
.WithOpenApi();

app.MapGet("GetAllReleasedVersionsPaginated", async (IGameVersionService gameVersionService, int page, int take) =>
    await gameVersionService.GetAllReleasedVersionsAsync(page: page, take: take).ConfigureAwait(false)
).WithName("GetAllReleasedVersionsPaginated")
.WithOpenApi();

app.MapGet("GetVersionDetails", async (IGameVersionService gameVersionService, string guid) =>
    await gameVersionService.GetVersionByGuidAsync(guid).ConfigureAwait(false)
).WithName("GetVersionDetails")
.WithOpenApi();

await app.RunAsync().ConfigureAwait(false);