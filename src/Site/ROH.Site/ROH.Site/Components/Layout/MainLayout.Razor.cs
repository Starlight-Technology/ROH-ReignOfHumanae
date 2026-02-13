using MudBlazor;

namespace ROH.Site.Components.Layout;


public static class RohTheme
{
    private static readonly MudTheme _darkTheme = new()
    {
        PaletteDark = new PaletteDark
        {
            AppbarText = "#e6c77a",
            PrimaryDarken = "#b38b3f",

            ActionDefault = "#c9a24d",
            ActionDisabled = "rgba(201,162,77,0.45)",
            ActionDisabledBackground = "rgba(201,162,77,0.12)",

            Background = "#070A0F",
            Surface = "#0E1522",

            Primary = "#C9A24D",          // dourado envelhecido
            PrimaryContrastText = "#0B0F14",

            Secondary = "#1E3A5F",        // azul aço ROH
            SecondaryContrastText = "#E6D3A1",

            Warning = "#8C1D18",          // carmesim
            WarningContrastText = "#F3E6C8",

            TextPrimary = "#E6D3A1",      // marfim
            TextSecondary = "#B8A878",

            LinesDefault = "#2A3446",
            Divider = "#2A3446",

            DrawerIcon = "#d9b15e",
            AppbarBackground = "#0B0F14",
            DrawerBackground = "#0B0F14"
        },

        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Cinzel", "Trajan Pro", "serif"],
                FontSize = "0.98rem",
                FontWeight = "400"
            },
            H6 = new H6Typography
            {
                FontFamily = ["Cinzel", "Trajan Pro", "serif"],
                FontWeight = "600",
                LetterSpacing = "0.18em"
            },
            Subtitle1 = new Subtitle1Typography
            {
                FontFamily = ["Cinzel", "Trajan Pro", "serif"],
                FontWeight = "500",
                LetterSpacing = "0.12em"
            }, 
        },

        LayoutProperties = new LayoutProperties
        {
            AppbarHeight = "64px",
            DrawerWidthLeft = "280px"
        }
    };

    public static MudTheme Dark { get; set; } = _darkTheme;
}
