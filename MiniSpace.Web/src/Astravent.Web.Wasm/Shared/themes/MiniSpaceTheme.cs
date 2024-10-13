using MudBlazor;

public static class MiniSpaceTheme
{
    public static MudTheme MiniSpaceCustomTheme = new MudTheme()
    {
        Palette = new PaletteLight()
        {
            Primary = "#0A66C2",  // LinkedIn blue for primary actions, giving a professional tone
            Secondary = "#005A9E", // Darker shade of blue for secondary actions, serious and clean
            Tertiary = "#FF8C00",  // A muted orange for accent and call-to-action elements
            AppbarText = Colors.Shades.White, // Clear white text for app bar
            AppbarBackground = "#F0F1F3", // LinkedIn blue for app bar background for uniformity
            Background = "#F4F4F4", // Soft light grey background for the entire application
            Surface = Colors.Shades.White, // White for cards and surfaces, providing contrast
            DrawerBackground = "#F0F1F3",  // Neutral light grey for drawer background, clean look
            DrawerText = "#323130", // Dark grey text for readability in drawer
            DrawerIcon = "#0078D4",  // Fluent UI blue for drawer icons, consistent with Microsoft style
            TextPrimary = "#1B1A19", // Almost black for primary text, making it clear and professional
            TextSecondary = "#595959",  // Soft grey for secondary text, not too overpowering
            ActionDefault = "#005A9E",  // Consistent blue for action buttons and interactive elements
            ActionDisabled = "#A6A6A6", // Muted grey for disabled actions, subtle and clean
            ActionDisabledBackground = "#E0E0E0", // Light grey background for disabled actions
            Success = "#107C10",   // Microsoft green for success, subtle yet modern
            Warning = "#F9B900",   // Bright yellow for warnings, noticeable but professional
            Error = "#D13438",     // Fluent-style red for errors, attention-grabbing but not harsh
            Info = "#605E5C",      // Neutral grey for informational elements
            LinesDefault = "#E1E1E1",  // Light grey for lines and borders, keeping the design clean
            Divider = "#DADADA",   // Soft grey dividers for structure, without standing out too much
            OverlayLight = "rgba(255, 255, 255, 0.85)",  // Subtle overlay for modals, maintaining clarity
            OverlayDark = "rgba(0, 0, 0, 0.5)",  // Slightly darker overlay for modals, clear contrast
            TableLines = "#E0E0E0", // Soft grey table lines to keep table components minimalistic
            HoverOpacity = 0.10f, // Slight hover effect for interactive elements
           
        },
        PaletteDark = new PaletteDark()
        {
            // Dark Mode Colors
            Primary = "#8AB4F8",  // Soft blue for dark mode
            Secondary = "#6C757D", // Greyish blue for secondary elements in dark mode
            AppbarBackground = "#212121", // Dark app bar for dark mode
            Background = "#121212", // Very dark background to ease the eyes
            Surface = "#1E1E1E", // Darker surface color to contrast with the background
            DrawerBackground = "#242424", // Dark drawer background to blend with dark theme
            DrawerText = "#E8EAED", // Light grey text for readability in dark mode
            TextPrimary = "#E8EAED", // White text for primary text readability
            TextSecondary = "#B0B0B0",  // Lighter grey for secondary text
            ActionDefault = "#8AB4F8",  // Blue for actions and interactive elements
            ActionDisabled = "#5F6368", // Soft grey for disabled actions
            Success = "#81C995",   // Light green for success
            Warning = "#F9AB00",   // Yellow for warnings
            Error = "#CF6679",     // Soft red for error messages
            Info = "#4285F4"       // Same cool blue for informational elements
        },
        Typography = new Typography()
        {
            Default = new Default()
            {
                FontFamily = new[] { "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" }, // Modern, readable font stack
                FontSize = "0.9375rem",  // 15px for a comfortable reading experience
                FontWeight = 400,        // Normal font weight for readability
            },
            H1 = new H1()
            {
                FontFamily = new[] { "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" },
                FontSize = "2rem", // Larger size for headings
                FontWeight = 600,
                LineHeight = 1.2,
            },
            H2 = new H2()
            {
                FontFamily = new[] { "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" },
                FontSize = "1.75rem",
                FontWeight = 500,
                LineHeight = 1.25
            },
            H3 = new H3()
            {
                FontFamily = new[] { "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "sans-serif" },
                FontSize = "1.5rem",
                FontWeight = 500
            },
            Button = new Button()
            {
                FontWeight = 600,
                TextTransform = "none", // Keeping natural text case for buttons
                LetterSpacing = ".03em" // Subtle spacing for better readability
            }
        },

        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "4px", // Slightly sharper corners for a modern feel
            DrawerWidthLeft = "250px" // Standard drawer width for navigation
        }
    };
}
