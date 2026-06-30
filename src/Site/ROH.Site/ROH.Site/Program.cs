using Blazored.LocalStorage;

using Corona.Theming;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using MudBlazor.Services;

using ROH.Site.Components;
using ROH.Site.Helpers;
using ROH.Site.Helpers.Components.Layout;
using ROH.Site.Services;

using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ServicesManager servicesManager = new();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.WebHost
    .ConfigureKestrel(
        options =>
        {
            options.ListenAnyIP(9010);
            options.Limits.MaxRequestBodySize = null;
        });

// Configure JWT authentication
string tokenKey = Environment.GetEnvironmentVariable("ROH_KEY_TOKEN") ?? "thisisaverysecurekeywith32charslong!";
builder.Services
    .AddAuthentication(
        options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(
        options => options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                ValidIssuer = "ROH.Services.Authentication.AuthService",
                ValidAudience = "ROH.Gateway"
            });

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddBlazoredLocalStorage();
//builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMudServices();
builder.Services.AddCoronaTheming(
    CoronaThemes.Dark(
        new CoronaThemeOverrides(
            Semantic: new CoronaSemanticTokenOverrides(
                ColorPrimary: "#c9a24d",
                SurfaceBackground: "#0b0f14",
                SurfaceBackgroundAlt: "#10162a",
                CardBackground: "#161a32",
                TextPrimary: "#e6d3a1",
                TextSecondary: "#d8caa8",
                BorderDefault: "rgba(201, 162, 77, 0.32)",
                FocusOutline: "rgba(201, 162, 77, 0.18)",
                ElevationCard: "0 14px 32px rgba(0, 0, 0, 0.62), inset 0 0 24px rgba(201, 162, 77, 0.05)",
                RadiusCard: "12px",
                SpacingCard: "1.25rem",
                SpacingCardHeader: "1.25rem",
                FontFamilyDefault: "Inter, system-ui, sans-serif",
                FontSizeHeading: "1rem",
                FontWeightHeading: "600"))));
builder.Services.AddScoped<DrawerState>();

servicesManager.ConfigureServices(builder.Services);

builder.Services.AddLocalization();

builder.Services.AddScoped<LanguageService>();

var app = builder.Build();

string[] supportedCultures = ["en", "pt", "es", "fr", "de", "it", "ja", "ko", "zh", "ru"];

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new("en"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
#if !DEBUG
app.UseHttpsRedirection();
#endif
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ROH.Site.Client._Imports).Assembly)
    .AddAdditionalAssemblies(typeof(CoronaThemeProvider).Assembly);

app.Run();
