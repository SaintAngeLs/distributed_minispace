using MudBlazor;

public static class MiniSpaceTheme
{
    public static MudTheme MiniSpaceCustomTheme = new MudTheme()
    {
        Palette = new Palette()
        {
            Primary = Colors.Indigo.Darken4, 
            Secondary = Colors.Indigo.Darken3, 
            AppbarBackground = Colors.Shades.White,
            Background = Colors.Grey.Lighten5
        }
    };
}
