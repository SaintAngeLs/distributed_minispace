using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Friends.E2ETests.Auth
{
    public class AuthResponse
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public string role { get; set; }
        public long expires { get; set; }
    }
}