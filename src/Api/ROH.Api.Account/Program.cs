//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using FluentValidation;

using ROH.Context.PostgreSQLContext;
using ROH.Interfaces;
using ROH.Interfaces.Repository.Account;
using ROH.Interfaces.Repository.Log;
using ROH.Interfaces.Services.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Mapping.Account;
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
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IValidator<UserModel>, UserModelValidator>();

builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();

// Auto Mapper Configurations
MapperConfiguration mappingConfig = new(
    mc =>
    {
        mc.AddProfile(new UserMapping());
        mc.AddProfile(new AccountMapping());
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

app.MapPost(
    "CreateNewUser",
    async (IUserService userService, UserModel model) => await userService.NewUserAsync(model).ConfigureAwait(false)
)
    .WithName("CreateNewUser")
    .WithOpenApi();

app.MapGet(
    "FindUserByEmail",
    async (IUserService userService, string email) => await userService.FindUserByEmailAsync(email).ConfigureAwait(false)
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

app.UseHttpsRedirection();

await app.RunAsync().ConfigureAwait(false);
