using ROH.Api.Log.Services;
using ROH.Context.Log;
using ROH.Context.Log.Interface;
using ROH.Context.Log.Repository;
using ROH.Service.Log;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Registry Interfaces

builder.Services.AddScoped<ILogContext, LogContext>();

builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapGrpcService<LogServiceImplementation>();

await app.RunAsync().ConfigureAwait(true);
