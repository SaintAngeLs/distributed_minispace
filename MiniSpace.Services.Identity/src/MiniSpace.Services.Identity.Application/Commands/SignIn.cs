using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class SignIn : ICommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DeviceType { get; set; }  
        public SignIn(string email, string password, string deviceType)
        {
            Email = email;
            Password = password;
            DeviceType = deviceType;  
        }
    }
}
