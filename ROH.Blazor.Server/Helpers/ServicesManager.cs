using ROH.Blazor.Server.Api;
using ROH.Blazor.Server.Interfaces.Api;
using ROH.Blazor.Server.Interfaces.Helpers;

namespace ROH.Blazor.Server.Helpers
{
    public class ServicesManager
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IVersionService, VersionService>();
            services.AddScoped<IVersionFileService, VersionFileService>();
            services.AddScoped<ISweetAlertService, SweetAlertService>();
        }
    }
}
