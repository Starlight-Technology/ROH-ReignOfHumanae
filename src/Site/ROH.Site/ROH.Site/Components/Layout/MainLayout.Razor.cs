using MudBlazor;

namespace ROH.Site.Components.Layout;


public static class RohTheme
{
    public static MudTheme Dark = new()
    {
        PaletteDark = new PaletteDark
        {
            Primary = "#c9a24d",        // Aged gold
            Secondary = "#3f6f98",      // Blue steel
            Background = "#05070f",
            Surface = "#0b0f1e",
            DrawerBackground = "#0a0e1a",
            AppbarBackground = "#121735",
            TextPrimary = "#eef0f6",
            TextSecondary = "#b5bccb",
            Divider = "rgba(201,162,77,0.18)",
            AppbarText = "#e6c77a",
            PrimaryDarken = "#b38b3f",
            DrawerIcon = "#d9b15e",
            ActionDefault = "#c9a24d",
            ActionDisabled = "rgba(201,162,77,0.45)",
            ActionDisabledBackground = "rgba(201,162,77,0.12)"
        },

        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = new[] { "Cinzel", "Trajan Pro", "serif" },
                FontSize = "0.98rem",
                FontWeight = 400
            },
            H6 = new H6Typography
            {
                FontFamily = new[] { "Cinzel", "Trajan Pro", "serif" },
                FontWeight = 600,
                LetterSpacing = "0.18em"
            },
            Subtitle1 = new Subtitle1Typography
            {
                FontFamily = new[] { "Cinzel", "Trajan Pro", "serif" },
                FontWeight = 500,
                LetterSpacing = "0.12em"
            }
        },

        LayoutProperties = new LayoutProperties
        {
            AppbarHeight = "64px",
            DrawerWidthLeft = "280px"
        }
    };
}
