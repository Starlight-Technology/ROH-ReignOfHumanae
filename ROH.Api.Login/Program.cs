using FluentValidation;

using ROH.Context.PostgreSQLContext;
using ROH.Interfaces;
using ROH.Interfaces.Repository.Account;
using ROH.Interfaces.Repository.Log;
using ROH.Interfaces.Services.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Repository.Account;
using ROH.Repository.Log;
using ROH.Services.Account;
using ROH.Services.ExceptionService;
using ROH.StandardModels.Account;
using ROH.Validations.Account;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registry Interfaces
builder.Services.AddScoped<ISqlContext, SqlContext>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IValidator<UserModel>, UserModelValidator>();
builder.Services.AddScoped<IValidator<LoginModel>, LoginModelValidator>();

builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.MapPost("Login", async (ILoginService loginService, LoginModel model) =>
    await loginService.Login(model)
).WithName("Login")
.WithOpenApi();

app.UseHttpsRedirection();

app.Run();
