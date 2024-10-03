using Paralax.Types;
using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Documents
{
    public class FriendDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid FriendId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public FriendState State { get; set; }
    }    
}
