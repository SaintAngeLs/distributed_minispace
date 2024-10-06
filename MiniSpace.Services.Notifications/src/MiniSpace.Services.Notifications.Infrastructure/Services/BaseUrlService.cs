using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MiniSpace.Services.Notifications.Application.Services;

namespace MiniSpace.Services.Notifications.Infrastructure.Services
{
    public class BaseUrlService : IBaseUrlService
    {
        private readonly IConfiguration _configuration;

        public BaseUrlService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetBaseUrl()
        {
            var environment = _configuration["ASPNETCORE_ENVIRONMENT"];
            return environment == "Production" 
                ? "https://astraven.com" 
                : "http://localhost:5606";
        }
    }
}