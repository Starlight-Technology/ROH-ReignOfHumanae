namespace ROH.Blazor.Server.Helpers
{
    public class ServicesManager
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<SweetAlertService>();
        }
    }
}
