using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Application.Dto
{
    public class FriendRequestDto
    {
        public Guid Id { get; set; }
        public Guid InviterId { get; set; }
        public Guid InviteeId { get; set; }
        public DateTime RequestedAt { get; set; }
        public FriendState State { get; set; }
        public Guid UserId { get; set; } 

    }
}
