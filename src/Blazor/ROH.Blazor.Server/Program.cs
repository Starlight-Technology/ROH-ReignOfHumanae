using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ROH.Blazor.Server.Helpers;
using MatBlazor;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ServicesManager servicesManager = new();

builder.Services.AddRazorPages();
builder.Services.AddMatBlazor();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
{
#if DEBUG
    options.DetailedErrors = true;
#endif
});

servicesManager.ConfigureServices(builder.Services);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(9010);
    options.Limits.MaxRequestBodySize = null;
});

// Configure JWT authentication
string tokenKey = Environment.GetEnvironmentVariable("ROH_KEY_TOKEN") ?? "thisisaverysecurekeywith32charslong!";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
        ValidIssuer = "ROH.Services.Authentication.AuthService",
        ValidAudience = "ROH.Gateway"
    };
});

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

#if RELEASE
app.UseHttpsRedirection();
#endif

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.RunAsync();