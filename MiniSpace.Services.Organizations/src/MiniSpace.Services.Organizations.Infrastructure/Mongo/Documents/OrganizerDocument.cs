using Convey.Types;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class OrganizerDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }
}