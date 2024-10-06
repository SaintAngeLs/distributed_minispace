using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astravent.Web.Wasm.Utilities
{
    public interface IIPAddressService
    {
        Task<string> GetClientIpAddressAsync();
    }
}