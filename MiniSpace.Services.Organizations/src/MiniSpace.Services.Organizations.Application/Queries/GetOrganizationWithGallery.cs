using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.Queries
{
    
    [ExcludeFromCodeCoverage]
    public class GetOrganizationWithGallery : IQuery<OrganizationGalleryDto>
    {
        public Guid OrganizationId { get; set; }
    }
    
}
