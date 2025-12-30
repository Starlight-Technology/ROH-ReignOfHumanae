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
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IValidator<UserModel>, UserModelValidator>();

builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();
builder.Services.AddScoped<ILogService, LogService>();

builder.WebHost
    .ConfigureKestrel(
        options =>
        {
            options.ListenAnyIP(
                9102,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2; // Supports both protocols
                });
            options.ListenAnyIP(
                9202,
                listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
        });

// Auto Mapper Configurations
MapperConfiguration mappingConfig = new(
    mc =>
    {
        mc.AddProfile(new UserMapping());
        mc.AddProfile(new AccountMapping());
    },
    NullLoggerFactory.Instance);

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.MapPost(
    "CreateNewUser",
    async (IUserService userService, UserModel model) => await userService.NewUserAsync(model).ConfigureAwait(false)
)
    .WithName("CreateNewUser")
    .WithOpenApi();

app.MapGet(
    "FindUserByEmail",
    async (IUserService userService, string email) => await userService.FindUserByEmailAsync(email)
        .ConfigureAwait(false)
)
    .WithName("FindUserByEmail")
    .WithOpenApi();

app.MapGet(
    "FindUserByUserName",
    async (IUserService userService, string userName) => await userService.FindUserByUserNameAsync(userName)
        .ConfigureAwait(false)
)
    .WithName("FindUserByUserName")
    .WithOpenApi();

app.MapGet(
    "GetUserByGuid",
    async (IUserService userService, Guid guid) => await userService.GetUserByGuidAsync(guid).ConfigureAwait(false)
)
    .WithName("GetUserByGuid")
    .WithOpenApi();

app.MapGet(
    "GetAccountByUserGuid",
    async (IAccountService accountService, Guid guid) => await accountService.GetAccountByUserGuidAsync(guid)
        .ConfigureAwait(false)
)
    .WithName("GetAccountByUserGuid")
    .WithOpenApi();

app.MapPut(
    "UpdateAccount",
    async (IAccountService accountService, AccountModel model) => await accountService.UpdateAccountAsync(model)
        .ConfigureAwait(false)
)
    .WithName("UpdateAccount")
    .WithOpenApi();

await app.RunAsync().ConfigureAwait(false);