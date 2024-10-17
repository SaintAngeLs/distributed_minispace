using System;
using Microsoft.AspNetCore.Http;
using MiniSpace.Services.Identity.Application.Services;
using MiniSpace.Services.Identity.Core.Entities;

namespace MiniSpace.Services.Identity.Infrastructure.Services
{
    public class DeviceInfoService : IDeviceInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeviceInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public DeviceInfo GetDeviceInfo()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }

            // Extract the User-Agent from the request headers
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var deviceType = userAgent.Contains("Mobile") ? "Mobile" : "Desktop";

            // Return the device information
            return new DeviceInfo
            {
                DeviceType = deviceType,
                UserAgent = userAgent
            };
        }
    }

}
