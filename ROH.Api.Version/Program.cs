using AutoMapper;

using FluentValidation;

using ROH.Context.PostgreSQLContext;
using ROH.Interfaces;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.Version;
using ROH.Mapper.Version;
using ROH.Repository.Version;
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
builder.Services.AddScoped<IGameVersionFileRepository, GameVersionFileRepository>();
builder.Services.AddScoped<IGameVersionRepository, GameVersionRepository>();

builder.Services.AddScoped<IGameVersionService, GameVersionService>();
builder.Services.AddScoped<IGameVersionFileService, GameVersionFileService>();

builder.Services.AddScoped<IValidator<GameVersionModel>, GameVersionModelValidation>();
builder.Services.AddScoped<IValidator<GameVersionFileModel>, GameVersionFileModelValidation>();

// Auto Mapper Configurations
MapperConfiguration mappingConfig = new(mc =>
{
    mc.AddProfile(new GameVersionFileMapping());
    mc.AddProfile(new GameVersionMapping());
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


app.MapPost("CreateNewVersion", async (IGameVersionService _gameVersionService, GameVersionModel model) =>
{
    return await _gameVersionService.NewVersion(model);
}).WithName("CreateNewVersion")
  .WithOpenApi();

app.MapGet("GetCurrentVersion", async (IGameVersionService _gameVersionService) =>
{
    return await _gameVersionService.GetCurrentVersion();
}).WithName("GetCurrentVersion")
  .WithOpenApi();

app.MapGet("GetAllVersionsPaginated", async (IGameVersionService _gameVersionService, int page, int take) =>
{
    return await _gameVersionService.GetAllVersions(page: page, take: take);
}).WithName("GetAllVersionsPaginated")
  .WithOpenApi();

app.MapGet("GetAllReleasedVersionsPaginated", async (IGameVersionService _gameVersionService, int page, int take) =>
{
    return await _gameVersionService.GetAllReleasedVersions(page: page, take: take);
}).WithName("GetAllReleasedVersionsPaginated")
  .WithOpenApi();

app.MapGet("GetVersionDetails", async (IGameVersionService _gameVersionService, string guid) =>
{
    return await _gameVersionService.GetVersionByGuid(guid);
}).WithName("GetVersionDetails")
  .WithOpenApi();

app.Run();