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
        private readonly IUserProfileViewsRepository _userProfileViewsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly ILogger<ViewUserProfileHandler> _logger;

        public ViewUserProfileHandler(
            IUserProfileViewsRepository userProfileViewsRepository,
            IHttpContextAccessor httpContextAccessor,
            IDeviceInfoService deviceInfoService,
            ILogger<ViewUserProfileHandler> logger)
        {
            _userProfileViewsRepository = userProfileViewsRepository;
            _httpContextAccessor = httpContextAccessor;
            _deviceInfoService = deviceInfoService;
            _logger = logger;
        }

        public async Task HandleAsync(ViewUserProfile command, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var deviceInfo = _deviceInfoService.GetDeviceInfo(httpContext);

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

            userViews.AddView(command.UserProfileId, DateTime.UtcNow, deviceInfo.IpAddress, deviceInfo.DeviceType, deviceInfo.OperatingSystem);

            await _userProfileViewsRepository.UpdateAsync(userViews);

            _logger.LogInformation($"User {command.UserId} viewed user profile {command.UserProfileId} from IP {deviceInfo.IpAddress} using {deviceInfo.DeviceType} ({deviceInfo.OperatingSystem}).");
        }
    }
}
