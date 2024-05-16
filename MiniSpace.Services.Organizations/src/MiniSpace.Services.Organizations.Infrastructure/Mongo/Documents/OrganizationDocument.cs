using Convey.Types;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    public class OrganizationDocument: IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Organizer> Organizers { get; set; }
        public IEnumerable<Organization> SubOrganizations { get; set; }
    }
}