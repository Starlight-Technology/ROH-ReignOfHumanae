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

app.MapPost("UploadFile", async (IGameVersionFileService _gameVersionFileService, GameVersionFileModel File) =>
{
    return await _gameVersionFileService.NewFile(File);
}).WithName("UploadFile")
  .WithOpenApi();

app.MapGet("GetAllVersionFiles", async (IGameVersionFileService _gameVersionFileService, string VersionGuid) =>
{
    return await _gameVersionFileService.GetFiles(VersionGuid);
}
).WithName("GetAllVersionFiles")
.WithOpenApi();

app.Run();