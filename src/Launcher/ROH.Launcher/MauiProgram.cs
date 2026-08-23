using Microsoft.Extensions.Logging;

using ROH.Launcher.Services;
using Corona.Theming;

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
        builder.Services.AddSingleton<Services.IFolderPicker, ROH.Launcher.Platforms.MacCatalyst.FolderPickerImplementation>();
#else
        // fallback to FilePicker-based directory selection
        builder.Services.AddSingleton<Services.IFolderPicker, Services.FilePickerFolderPicker>();
#endif
        // Gateway/Api helpers
        builder.Services.AddHttpClient("GatewayClient");
        builder.Services.AddSingleton<ApiHelper>();
        builder.Services.AddSingleton<UpdaterService>();

        // Corona theming with ROH brand colors (dark theme with gold accents)
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

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
