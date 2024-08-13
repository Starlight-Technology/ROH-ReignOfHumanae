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
MapperConfiguration mappingConfig = new(mc =>
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

app.MapPost("CreateNewUser", async (IUserService _userService, UserModel model) =>
    await _userService.NewUser(model).ConfigureAwait(false)
).WithName("CreateNewUser")
.WithOpenApi();

app.MapGet("FindUserByEmail", async (IUserService _userService, string email) =>
    await _userService.FindUserByEmail(email).ConfigureAwait(false)
).WithName("FindUserByEmail")
.WithOpenApi();

app.MapGet("FindUserByUserName", async (IUserService _userService, string userName) =>
    await _userService.FindUserByUserName(userName).ConfigureAwait(false)
).WithName("FindUserByUserName")
.WithOpenApi();

app.MapGet("GetUserByGuid", async (IUserService _userService, Guid guid) =>
    await _userService.GetUserByGuid(guid).ConfigureAwait(false)
).WithName("GetUserByGuid")
.WithOpenApi();

app.MapGet("GetAccountByUserGuid", async (IAccountService _accountService, Guid guid) =>
    await _accountService.GetAccountByUserGuid(guid).ConfigureAwait(false)
).WithName("GetAccountByUserGuid")
.WithOpenApi();

app.MapPut("UpdateAccount", async (IAccountService _accountService, AccountModel model) =>
    await _accountService.UpdateAccount(model).ConfigureAwait(false)
).WithName("UpdateAccount")
.WithOpenApi();

app.UseHttpsRedirection();

await app.RunAsync().ConfigureAwait(false);
