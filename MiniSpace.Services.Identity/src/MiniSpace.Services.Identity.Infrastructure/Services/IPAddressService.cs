using System;
using Microsoft.AspNetCore.Http;
using MiniSpace.Services.Identity.Application.Services;

namespace MiniSpace.Services.Identity.Infrastructure.Services
{
    public class IPAddressService : IIPAddressService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IPAddressService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string GetIPAddress()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                return "Unknown IP";
            }

            var forwardedHeader = context.Request.Headers["X-Forwarded-For"].ToString();
            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                var ipAddresses = forwardedHeader.Split(',');
                if (ipAddresses.Length > 0)
                {
                    return ipAddresses[0].Trim();
                }
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
        }
    }
}
