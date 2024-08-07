using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Services.Clients
{
    public interface IOrganizationsServiceClient
    {
        Task<OrganizationDto> GetAsync(Guid organizationId);
        Task<IEnumerable<Guid>> GetAllChildrenOrganizations(Guid organizationId, Guid rootId);
    }
}