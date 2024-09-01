using Microsoft.AspNetCore.Http;
using MiniSpace.Services.Students.Core.Entities;
using System;

namespace MiniSpace.Services.Students.Application.Services
{
    public class DeviceInfoService : IDeviceInfoService
    {
        public DeviceInfo GetDeviceInfo(HttpContext httpContext)
        {
            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            var userAgent = httpContext.Request.Headers["User-Agent"].ToString().ToLower();
            var deviceType = userAgent.Contains("mobile") ? "Mobile" : "Computer";
            var operatingSystem = userAgent.Contains("windows") ? "Windows" :
                                  userAgent.Contains("mac") ? "MacOS" :
                                  userAgent.Contains("android") ? "Android" :
                                  userAgent.Contains("iphone") ? "iOS" :
                                  userAgent.Contains("linux") ? "Linux" : "Unknown";

            return new DeviceInfo
            {
                IpAddress = ipAddress,
                DeviceType = deviceType,
                OperatingSystem = operatingSystem
            };
        }
    }
}
