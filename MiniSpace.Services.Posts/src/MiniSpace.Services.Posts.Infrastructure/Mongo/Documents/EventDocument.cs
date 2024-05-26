using System.Diagnostics.CodeAnalysis;
using Convey.Types;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class EventDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrganizerId { get; set; }
    }    
}
