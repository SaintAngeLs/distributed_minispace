using System.Net;
using Microsoft.AspNetCore.Http;

namespace MiniSpace.Services.Identity.Application.Services
{
    public interface IIPAddressService
    {
        string GetIPAddress();
    }
}