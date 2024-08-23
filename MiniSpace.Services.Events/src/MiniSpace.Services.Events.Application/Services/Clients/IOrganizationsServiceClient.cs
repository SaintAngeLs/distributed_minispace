using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Services.Clients
{
    public interface IOrganizationsServiceClient
    {
        Task<OrganizationDetailsDto> GetAsync(Guid organizationId);
        Task<IEnumerable<Guid>> GetAllChildrenOrganizations(Guid organizationId);
    }
}