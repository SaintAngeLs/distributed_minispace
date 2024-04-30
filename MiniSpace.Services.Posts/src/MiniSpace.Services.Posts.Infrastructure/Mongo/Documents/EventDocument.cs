using Convey.Types;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    public class EventDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrganizerId { get; set; }
    }    
}
