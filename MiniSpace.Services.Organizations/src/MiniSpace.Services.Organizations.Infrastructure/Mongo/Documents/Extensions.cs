using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Organization AsEntity(this OrganizationDocument document)
            => new Organization(document.Id, document.Name, document.ParentId, document.Organizers);
        
        public static OrganizationDocument AsDocument(this Organization entity)
            => new OrganizationDocument()
            {
                Id = entity.Id,
                Name = entity.Name,
                ParentId = entity.ParentId,
                Organizers = entity.Organizers
            };
        
        public static OrganizationDto AsDto(this OrganizationDocument document)
            => new OrganizationDto()
            {
                Id = document.Id,
                Name = document.Name,
                ParentId = document.ParentId,
                MemberCount = document.Organizers.Count()
            };
        
        public static Organizer AsEntity(this OrganizerDocument document)
            => new Organizer(document.Id);
        
        public static OrganizerDocument AsDocument(this Organizer entity)
            => new OrganizerDocument()
            {
                Id = entity.Id
            };
    }    
}
