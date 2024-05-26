using Convey.Types;
using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]

    public class OrganizationDocument: IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Organizer> Organizers { get; set; }
        public IEnumerable<OrganizationDocument> SubOrganizations { get; set; }
    }
}