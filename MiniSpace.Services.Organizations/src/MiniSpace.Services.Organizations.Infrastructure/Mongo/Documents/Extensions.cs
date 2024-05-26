using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]

    public static class Extensions
    {
        public static Organization AsEntity(this OrganizationDocument document)
            => new Organization(document.Id, document.Name, document.Organizers, document.SubOrganizations.Select(o => o.AsEntity()));
        
        public static OrganizationDocument AsDocument(this Organization entity)
            => new OrganizationDocument()
            {
                Id = entity.Id,
                Name = entity.Name,
                Organizers = entity.Organizers,
                SubOrganizations = entity.SubOrganizations.Select(o => o.AsDocument())
            };
        
        public static OrganizationDto AsDto(this OrganizationDocument document, Guid rootId)
            => new OrganizationDto()
            {
                Id = document.Id,
                Name = document.Name,
                RootId = rootId
            };
        
        public static OrganizationDetailsDto AsDetailsDto(this OrganizationDocument document, Guid rootId)
            => new OrganizationDetailsDto()
            {
                Id = document.Id,
                Name = document.Name,
                RootId = rootId,
                Organizers = document.Organizers.Select(x => x.Id)
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
