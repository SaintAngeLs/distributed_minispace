using MudBlazor;

public static class MiniSpaceTheme
{
    public static MudTheme MiniSpaceCustomTheme = new MudTheme()
    {
        Palette = new PaletteLight()
        {
            Primary = Colors.Indigo.Darken4,
            Secondary = Colors.Indigo.Darken3,
            AppbarText = Colors.Indigo.Darken4,
            AppbarBackground = Colors.Shades.White,
            Background = Colors.Grey.Lighten5,
            Surface = "#FFFFFF",
            DrawerBackground = Colors.Grey.Lighten3,
            DrawerText = Colors.Grey.Darken3,
            TextPrimary = Colors.Shades.Black,
            TextSecondary = Colors.Grey.Darken2,
            ActionDefault = Colors.Indigo.Lighten1,
            ActionDisabled = Colors.Grey.Lighten2,
            Success = Colors.Green.Default,
            Warning = Colors.Orange.Default,
            Error = Colors.Red.Default,
            Info = Colors.Blue.Default
        },
        PaletteDark = new PaletteDark()
        {
            // Dark Mode Colors
            Primary = Colors.Indigo.Darken1,
            Secondary = Colors.Blue.Darken4,
            AppbarBackground = "#1F1F1F", // Dark app bar
            Background = "#0d0f17", // Very dark background
            Surface = "#212121", // Slightly lighter surface color
            DrawerBackground = "#1B1B1B", // Dark drawer background
            Dark = "#000000", // Dark drawer background
            DrawerText = Colors.Grey.Lighten2,
            TextPrimary = Colors.Shades.White,
            TextSecondary = Colors.Grey.Lighten2,
            ActionDefault = Colors.Indigo.Accent2,
            ActionDisabled = Colors.BlueGrey.Darken3,
            Success = Colors.Green.Accent3,
            Warning = Colors.Orange.Accent3,
            Error = Colors.Red.Accent3,
            Info = Colors.Blue.Accent3
        }
    };
}
