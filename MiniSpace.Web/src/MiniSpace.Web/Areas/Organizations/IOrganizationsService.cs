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
        Task<OrganizationDto> GetOrganizationAsync(Guid organizationId, Guid rootId);
        Task<OrganizationDetailsDto> GetOrganizationDetailsAsync(Guid organizationId, Guid rootId);
        Task<IEnumerable<OrganizationDto>> GetOrganizerOrganizationsAsync(Guid organizerId);
        Task<IEnumerable<OrganizationDto>> GetRootOrganizationsAsync();
        Task<IEnumerable<OrganizationDto>> GetChildrenOrganizationsAsync(Guid organizationId, Guid rootId);
        Task<IEnumerable<Guid>> GetAllChildrenOrganizationsAsync(Guid organizationId, Guid rootId);
        Task<HttpResponse<object>> CreateOrganization(Guid organizationId, string name, Guid rootId, Guid parentId);
        Task<HttpResponse<object>> CreateRootOrganization(Guid organizationId, string name);
        Task AddOrganizerToOrganization(Guid rootOrganizationId, Guid organizationId, Guid organizerId);
        Task RemoveOrganizerFromOrganization(Guid rootOrganizationId, Guid organizationId, Guid organizerId);
    }    
}
