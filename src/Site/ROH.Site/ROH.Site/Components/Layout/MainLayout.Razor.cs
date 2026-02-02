using MudBlazor;

namespace ROH.Site.Components.Layout;


public static class RohTheme
{
    public static MudTheme Dark = new()
    {
        PaletteDark = new PaletteDark
        {
            Primary = "#c9a24d",        // Aged gold
            Secondary = "#3a6ea5",      // Blue accent
            Background = "#070a14",
            Surface = "#0b0f1e",
            DrawerBackground = "#0b0f1e",
            AppbarBackground = "#121735",
            TextPrimary = "#e6e8ef",
            TextSecondary = "#b8bccb",
            Divider = "rgba(255,255,255,0.08)",
            AppbarText = "#c9a24d",
            PrimaryDarken = "#c9a24d",
            DrawerIcon = "#c9a24d"
        },

        LayoutProperties = new LayoutProperties
        {
            AppbarHeight = "64px",
            DrawerWidthLeft = "260px"
        },
        Typography = new Typography
        {
            
        }
    };
}
