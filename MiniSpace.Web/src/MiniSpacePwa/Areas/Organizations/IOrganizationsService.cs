using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpacePwa.DTO;
using MiniSpacePwa.DTO.Wrappers;
using MiniSpacePwa.HttpClients;

namespace MiniSpacePwa.Areas.Organizations
{
    public interface IOrganizationsService
    {
        Task<OrganizationDto> GetOrganizationAsync(Guid organizationId, Guid rootId);
        Task<OrganizationDetailsDto> GetOrganizationDetailsAsync(Guid organizationId, Guid rootId);
        Task<IEnumerable<OrganizationDto>> GetOrganizerOrganizationsAsync(Guid organizerId);
        Task<IEnumerable<OrganizationDto>> GetRootOrganizationsAsync();
        Task<IEnumerable<OrganizationDto>> GetChildrenOrganizationsAsync(Guid organizationId, Guid rootId);
        Task<IEnumerable<Guid>> GetAllChildrenOrganizationsAsync(Guid organizationId, Guid rootId);
        Task<HttpResponse<object>> CreateOrganizationAsync(Guid organizationId, string name, Guid rootId, Guid parentId);
        Task<HttpResponse<object>> CreateRootOrganizationAsync(Guid organizationId, string name);
        Task DeleteOrganizationAsync(Guid organizationId, Guid rootId);
        Task AddOrganizerToOrganizationAsync(Guid rootOrganizationId, Guid organizationId, Guid organizerId);
        Task RemoveOrganizerFromOrganizationAsync(Guid rootOrganizationId, Guid organizationId, Guid organizerId);
    }    
}
