using Microsoft.AspNetCore.Http;
using MiniSpace.Services.Students.Core.Entities;
using System;

namespace MiniSpace.Services.Students.Application.Services
{
    public interface IDeviceInfoService
    {
        DeviceInfo GetDeviceInfo(HttpContext httpContext);
    }
}