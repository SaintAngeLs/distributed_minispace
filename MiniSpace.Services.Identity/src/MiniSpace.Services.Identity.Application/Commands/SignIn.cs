using Convey.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    [Contract]
    public class SignIn : ICommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DeviceType { get; set; }  
        public string IpAddress { get; set; }  
        public SignIn(string email, string password, string deviceType, string ipAddress)
        {
            Email = email;
            Password = password;
            DeviceType = deviceType;  
            IpAddress = ipAddress;
        }
    }
}
