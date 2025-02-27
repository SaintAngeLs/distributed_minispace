using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Astravent.Web.Wasm.Areas.Students;
using Astravent.Web.Wasm.DTO;
using Microsoft.AspNetCore.Components.Authorization;

namespace Astravent.Web.Wasm.Areas.Layout
{
    public class LayoutService : ILayoutService
    {
        private readonly IStudentsService _studentsService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public bool IsRTL { get; private set; } = false;
        public FrontendVersion CurrentFrontendVersion { get; private set; } = FrontendVersion.LightMode;

        public LayoutService(IStudentsService studentsService, AuthenticationStateProvider authenticationStateProvider)
        {
            _studentsService = studentsService;
            _authenticationStateProvider = authenticationStateProvider;
            // Load user preferences asynchronously on initialization.
            _ = LoadUserPreferencesAsync();
        }

        /// <summary>
        /// Loads the user settings from the student service and updates the UI version.
        /// </summary>
        private async Task LoadUserPreferencesAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated)
            {
                var userId = GetUserId(authState.User);
                if (userId != Guid.Empty)
                {
                    var settings = await _studentsService.GetUserSettingsAsync(userId);
                    CurrentFrontendVersion = settings.FrontendVersion;
                }
            }
        }

        /// <summary>
        /// Toggles the layout direction (Right-to-Left vs. Left-to-Right).
        /// </summary>
        public void ToggleRightToLeft()
        {
            IsRTL = !IsRTL;
        }

        /// <summary>
        /// Cycles the UI theme mode among LightMode, DarkMode, and SystemMode,
        /// and updates the student settings accordingly.
        /// </summary>
        public async Task CycleThemeModeAsync()
        {
            CurrentFrontendVersion = CurrentFrontendVersion switch
            {
                FrontendVersion.LightMode => FrontendVersion.DarkMode,
                FrontendVersion.DarkMode  => FrontendVersion.SystemMode,
                FrontendVersion.SystemMode => FrontendVersion.LightMode,
                _ => FrontendVersion.LightMode,
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated)
            {
                var userId = GetUserId(authState.User);
                if (userId != Guid.Empty)
                {
                    var settings = await _studentsService.GetUserSettingsAsync(userId);
                    settings.FrontendVersion = CurrentFrontendVersion;
                    await _studentsService.UpdateUserSettingsAsync(userId, settings);
                }
            }
        }

        /// <summary>
        /// Extracts the user ID from the ClaimsPrincipal.
        /// Adjust the claim type as necessary.
        /// </summary>
        private Guid GetUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(c => c.Type == "sub" || c.Type == "userid");
            return (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId)) 
                ? userId 
                : Guid.Empty;
        }
    }
}
