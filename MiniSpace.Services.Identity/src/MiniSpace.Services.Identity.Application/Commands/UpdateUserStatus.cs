using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class UpdateUserStatus : ICommand
    {
        public Guid UserId { get; set; }
        public bool IsOnline { get; set; }
        public string DeviceType { get; set; }

        public UpdateUserStatus(Guid userId, bool isOnline, string deviceType)
        {
            UserId = userId;
            IsOnline = isOnline;
            DeviceType = deviceType;
        }
    }
}
