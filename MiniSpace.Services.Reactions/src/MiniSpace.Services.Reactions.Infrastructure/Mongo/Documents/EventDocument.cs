using Convey.Types;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents
{
    public class EventDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrganizerId {get;set;}
    }    
}
