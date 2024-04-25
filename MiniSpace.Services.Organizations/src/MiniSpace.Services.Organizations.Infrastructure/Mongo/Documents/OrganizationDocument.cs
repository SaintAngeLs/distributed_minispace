using Convey.Types;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    public class OrganizationDocument:IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }
    }
}