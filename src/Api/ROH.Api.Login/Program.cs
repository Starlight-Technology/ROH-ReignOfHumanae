//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using FluentValidation;

using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging.Abstractions;

using ROH.Context.Account;
using ROH.Context.Account.Interface;
using ROH.Context.Account.Repository;
using ROH.Mapping.Account;
using ROH.Service.Account;
using ROH.Service.Account.Interface;
using ROH.Service.Authentication;
using ROH.Service.Authentication.Interface;
using ROH.Service.Exception;
using ROH.Service.Exception.Communication;
using ROH.Service.Exception.Interface;
using ROH.StandardModels.Account;
using ROH.Validations.Account;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registry Interfaces
builder.Services.AddScoped<IAccountContext, AccountContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IValidator<UserModel>, UserModelValidator>();
builder.Services.AddScoped<IValidator<LoginModel>, LoginModelValidator>();

builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();
builder.Services.AddScoped<ILogService, LogService>();

// Auto Mapper Configurations
MapperConfiguration mappingConfig = new(
    mc =>
    {
        mc.AddProfile(new UserMapping());
        mc.AddProfile(new AccountMapping());
    }, NullLoggerFactory.Instance);

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.WebHost
    .ConfigureKestrel(
        options =>
        {
            options.ListenAnyIP(
                9103,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                });
            options.ListenAnyIP(
                9203,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
        });

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.MapPost(
    "Login",
    async (ILoginService loginService, LoginModel model) => await loginService.LoginAsync(model).ConfigureAwait(false)
)
    .WithName("Login")
    .WithOpenApi();

await app.RunAsync().ConfigureAwait(false);
