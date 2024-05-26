using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Application.Dto
{
    public class FriendRequestDto
    {
        public Guid Id { get; set; }
        public Guid InviterId { get; set; }
        public Guid InviteeId { get; set; }
        public DateTime RequestedAt { get; set; }
        public FriendState State { get; set; }
        public Guid StudentId { get; set; } 

    }
}
