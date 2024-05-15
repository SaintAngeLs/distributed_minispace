using Convey.Types;
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
        public string InviterFullName { get; set; }
        public string InviterEmail { get; set; }
        public string InviteeFullName { get; set; }
        public string InviteeEmail { get; set; }
    }  
}
