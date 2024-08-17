using ROH.Blazor.Server.Api;
using ROH.Blazor.Server.Interfaces.Api;
using ROH.Blazor.Server.Interfaces.Helpers;

namespace ROH.Blazor.Server.Helpers;

public class ServicesManager
{
    public void ConfigureServices(IServiceCollection services)
    {
        _ = services.AddScoped<IVersionService, VersionService>();
        _ = services.AddScoped<IVersionFileService, VersionFileService>();
        _ = services.AddScoped<ISweetAlertService, SweetAlertService>();
        _ = services.AddScoped<IDownloadFileService, DownloadFileService>();
        _ = services.AddScoped<IAccountService, AccountService>();
        _ = services.AddScoped<ICustomAuthenticationStateProvider, CustomAuthenticationStateProvider>();
    }
}