using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using MiniSpace.Services.Students.Application.Commands;
using MiniSpace.Services.Students.Application.Services;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class ViewUserProfileHandler : ICommandHandler<ViewUserProfile>
    {
        private readonly IUserProfileViewsForUserRepository _userProfileViewsForUserRepository;
        private readonly IUserViewingProfilesRepository _userViewingProfilesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly ILogger<ViewUserProfileHandler> _logger;

        public ViewUserProfileHandler(
            IUserProfileViewsForUserRepository userProfileViewsForUserRepository,
            IUserViewingProfilesRepository userViewingProfilesRepository,
            IHttpContextAccessor httpContextAccessor,
            IDeviceInfoService deviceInfoService,
            ILogger<ViewUserProfileHandler> logger)
        {
            _userProfileViewsForUserRepository = userProfileViewsForUserRepository;
            _userViewingProfilesRepository = userViewingProfilesRepository;
            _httpContextAccessor = httpContextAccessor;
            _deviceInfoService = deviceInfoService;
            _logger = logger;
        }

        public async Task HandleAsync(ViewUserProfile command, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var deviceInfo = _deviceInfoService.GetDeviceInfo(httpContext);

            // Handle views of the user profile being viewed
            var userViewsForUser = await _userProfileViewsForUserRepository.GetAsync(command.UserProfileId);
            if (userViewsForUser == null)
            {
                userViewsForUser = new UserProfileViewsForUser(command.UserProfileId, Enumerable.Empty<UserProfileView>());
            }

            userViewsForUser.AddView(command.UserId, DateTime.UtcNow, deviceInfo.IpAddress, deviceInfo.DeviceType, deviceInfo.OperatingSystem);
            await _userProfileViewsForUserRepository.UpdateAsync(userViewsForUser);

            // Handle views by the user viewing profiles
            var userViewingProfiles = await _userViewingProfilesRepository.GetAsync(command.UserId);
            if (userViewingProfiles == null)
            {
                userViewingProfiles = new UserViewingProfiles(command.UserId, Enumerable.Empty<UserProfileView>());
            }

            userViewingProfiles.AddViewedProfile(command.UserProfileId, DateTime.UtcNow, deviceInfo.IpAddress, deviceInfo.DeviceType, deviceInfo.OperatingSystem);
            await _userViewingProfilesRepository.UpdateAsync(userViewingProfiles);

            _logger.LogInformation($"User {command.UserId} viewed user profile {command.UserProfileId} from IP {deviceInfo.IpAddress} using {deviceInfo.DeviceType} ({deviceInfo.OperatingSystem}).");
        }
    }
}
