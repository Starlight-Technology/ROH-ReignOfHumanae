using FluentValidation;

using ROH.Context.PostgreSQLContext;
using ROH.Domain.Version;
using ROH.Interfaces;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.Version;
using ROH.Repository.Version;
using ROH.Services.Version;
using ROH.Validations.Version;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddScoped<IValidator<GameVersion>, GameVersionValidator>();
builder.Services.AddScoped<IValidator<GameVersionFile>, GameVersionFileValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/CreateNewVersion", () =>
{

})
.WithName("CreateNewVersion")
.WithOpenApi();

app.MapGet("/GetCurrentVersion", async (IGameVersionService _gameVersionService)  =>
{
    return await _gameVersionService.GetCurrentVersion();
}).WithName("GetCurrentVersion")
  .WithOpenApi();

app.Run();


