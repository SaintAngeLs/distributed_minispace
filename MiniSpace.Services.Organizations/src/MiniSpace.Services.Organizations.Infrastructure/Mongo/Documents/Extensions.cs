using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Organization AsEntity(this OrganizationDocument document)
            => new Organization(document.Id, document.Name, document.ParentId);
        
        public static OrganizationDocument AsDocument(this Organization entity)
            => new OrganizationDocument()
            {
                Id = entity.Id,
                Name = entity.Name,
                ParentId = entity.ParentId
            };
        
        public static OrganizationDto AsDto(this OrganizationDocument document)
            => new OrganizationDto()
            {
                Id = document.Id,
                Name = document.Name,
                ParentId = document.ParentId
            };
    }    
}
