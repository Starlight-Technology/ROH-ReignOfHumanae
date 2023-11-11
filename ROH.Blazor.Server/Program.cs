using ROH.Blazor.Server.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var servicesManager = new ServicesManager();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

servicesManager.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();