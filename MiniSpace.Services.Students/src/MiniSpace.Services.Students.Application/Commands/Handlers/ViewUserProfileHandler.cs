using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using MiniSpace.Services.Posts.Application.Commands;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class ViewUserProfileHandler : ICommandHandler<ViewUserProfile>
    {
        private readonly IUserProfileViewsRepository _userProfileViewsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ViewUserProfileHandler> _logger;

        public ViewUserProfileHandler(
            IUserProfileViewsRepository userProfileViewsRepository,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ViewUserProfileHandler> logger)
        {
            _userProfileViewsRepository = userProfileViewsRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task HandleAsync(ViewUserProfile command, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            var userAgent = httpContext.Request.Headers["User-Agent"].ToString().ToLower();
            var deviceType = userAgent.Contains("mobile") ? "Mobile" : "Computer";
            var operatingSystem = userAgent.Contains("windows") ? "Windows" :
                                userAgent.Contains("mac") ? "MacOS" :
                                userAgent.Contains("android") ? "Android" :
                                userAgent.Contains("iphone") ? "iOS" :
                                userAgent.Contains("linux") ? "Linux" : "Unknown";

            var userViews = await _userProfileViewsRepository.GetAsync(command.UserId);
            if (userViews == null)
            {
                userViews = new UserProfileViews(command.UserId, Enumerable.Empty<View>());
            }

            var existingView = userViews.Views.FirstOrDefault(v => v.UserProfileId == command.UserProfileId);
            if (existingView != null)
            {
                userViews.RemoveView(command.UserProfileId);
            }

            userViews.AddView(command.UserProfileId, DateTime.UtcNow, ipAddress, deviceType, operatingSystem);

            await _userProfileViewsRepository.UpdateAsync(userViews);

            _logger.LogInformation($"User {command.UserId} viewed user profile {command.UserProfileId} from IP {ipAddress} using {deviceType} ({operatingSystem}).");
        }

    }
}
