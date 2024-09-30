using Paralax.Types;
using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Documents
{
    public class FriendRequestDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid InviterId { get; set; }
        public Guid InviteeId { get; set; }
        public DateTime RequestedAt { get; set; }
        public FriendState State { get; set; }
    }    
}
