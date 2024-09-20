using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Application.Dto
{
    public class FriendDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
        public DateTime CreatedAt { get; set; }
        public FriendState State { get; set; }
    }
}
