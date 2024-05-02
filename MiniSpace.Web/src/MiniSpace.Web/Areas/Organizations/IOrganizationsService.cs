using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Organizations
{
    public interface IOrganizationsService
    {
        Task<OrganizationDto> GetOrganizationAsync(Guid organizationId);
        Task<OrganizationDetailsDto> GetOrganizationDetailsAsync(Guid organizationId);
        Task<IEnumerable<OrganizationDto>> GetOrganizerOrganizationsAsync(Guid organizerId);
        Task<IEnumerable<OrganizationDto>> GetRootOrganizationsAsync();
        Task<IEnumerable<OrganizationDto>> GetChildrenOrganizationsAsync(Guid organizationId);
        Task<HttpResponse<object>> AddOrganization(Guid organizationId, string name, string parentId);
        Task AddOrganizerToOrganization(Guid organizationId, Guid organizerId);
        Task RemoveOrganizerFromOrganization(Guid organizationId, Guid organizerId);
    }    
}
