using Grpc.Core;

using ROH.Api.Log.Services;
using ROH.Context.Log.Interface;
using ROH.Context.Log.Repository;
using ROH.Service.Log;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddGrpc();
builder.Services.AddScoped<ILogRepository, LogRepository>(); // Your repository implementation


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapGrpcService<LogServiceImplementation>();

await app.RunAsync().ConfigureAwait(true);


