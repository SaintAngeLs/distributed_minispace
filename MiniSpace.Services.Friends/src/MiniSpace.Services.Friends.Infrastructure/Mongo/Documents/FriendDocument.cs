using Convey.Types;
using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Documents
{
    public class FriendDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid FriendId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public FriendState State { get; set; }

        //  public string Email { get; set; }          
        // public string FirstName { get; set; }     
        // public string LastName { get; set; }  
    }    
}
