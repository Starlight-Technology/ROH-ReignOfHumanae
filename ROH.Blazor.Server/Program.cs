using ROH.Blazor.Server.Helpers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ServicesManager servicesManager = new();

builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });


servicesManager.ConfigureServices(builder.Services);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(9010);
    options.Limits.MaxRequestBodySize = null;
});

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

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();