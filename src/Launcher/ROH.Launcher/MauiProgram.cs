using Microsoft.Extensions.Logging;

using ROH.Launcher.Services;
using Microsoft.Maui.Storage;

namespace ROH.Launcher;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        // services
        builder.Services.AddSingleton<Services.AlertService>();
        builder.Services.AddSingleton<Services.ConfirmService>();
        builder.Services.AddSingleton<Services.SettingsService>();
        // Launcher state service used by launcher UI (login, update progress, settings)
        builder.Services.AddSingleton<Services.LauncherState>();
        // register folder picker implementations
        // platform-specific implementations are in Platforms folders and will be added by DI here
#if WINDOWS
        builder.Services.AddSingleton<Services.IFolderPicker, ROH.Launcher.Platforms.Windows.FolderPickerImplementation>();
#elif MACCATALYST || MACOS
        builder.Services.AddSingleton<Services.IFolderPicker, ROH.Launcher.Platforms.Mac.FolderPickerImplementation>();
#else
        // fallback to FilePicker-based directory selection
        builder.Services.AddSingleton<Services.IFolderPicker, Services.FilePickerFolderPicker>();
#endif
        // Gateway/Api helpers
        builder.Services.AddHttpClient("GatewayClient");
        builder.Services.AddSingleton<ApiHelper>();
        builder.Services.AddSingleton<UpdaterService>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
