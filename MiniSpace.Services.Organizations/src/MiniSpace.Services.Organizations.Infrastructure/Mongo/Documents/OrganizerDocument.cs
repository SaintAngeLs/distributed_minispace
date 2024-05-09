using Convey.Types;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    public class OrganizerDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }
}