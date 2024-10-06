using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.Areas.Organizations.CommandsDto;
using Astravent.Web.Wasm.Areas.PagedResult;
using Astravent.Web.Wasm.DTO.Organizations;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Organizations
{
    public interface IOrganizationsService
    {
        Task<OrganizationDto> GetOrganizationAsync(Guid organizationId);
        Task<OrganizationDetailsDto> GetOrganizationDetailsAsync(Guid organizationId);
        Task<IEnumerable<OrganizationDto>> GetRootOrganizationsAsync();
        Task<PagedResult<OrganizationDto>> GetChildrenOrganizationsAsync(Guid organizationId, int page, int pageSize);
        Task<OrganizationGalleryUsersDto> GetOrganizationWithGalleryAndUsersAsync(Guid organizationId);
        Task<IEnumerable<Guid>> GetAllChildrenOrganizationsAsync(Guid organizationId);
        Task<HttpResponse<object>> CreateOrganizationAsync(CreateOrganizationDto command);
        Task<HttpResponse<object>> CreateSubOrganizationAsync(Guid parentOrganizationId, CreateSubOrganizationCommand command);
        Task DeleteOrganizationAsync(Guid organizationId);
        Task<HttpResponse<object>> CreateOrganizationRoleAsync(Guid organizationId, CreateOrganizationRoleCommand command);
        Task InviteUserToOrganizationAsync(Guid organizationId, InviteUserToOrganizationCommand command);
        Task AssignRoleToMemberAsync(Guid organizationId, Guid memberId, AssignRoleToMemberCommand command);
        Task<HttpResponse<object>>  UpdateRolePermissionsAsync(Guid organizationId, Guid roleId, UpdateRolePermissionsCommand command);
        Task SetOrganizationPrivacyAsync(Guid organizationId, SetOrganizationPrivacyCommand command);
        Task<HttpResponse<object>> UpdateOrganizationSettingsAsync(Guid organizationId, UpdateOrganizationSettingsCommand command);
        Task SetOrganizationVisibilityAsync(Guid organizationId, SetOrganizationVisibilityCommand command);
        Task ManageFeedAsync(Guid organizationId, ManageFeedCommand command);
        Task<HttpResponse<object>> UpdateOrganizationAsync(Guid organizationId, UpdateOrganizationCommand command);
        Task<PagedResult<OrganizationDto>> GetPaginatedUserOrganizationsAsync(Guid userId, int page, int pageSize);
        Task<IEnumerable<RoleDto>> GetOrganizationRolesAsync(Guid organizationId);
        Task<PagedResult<OrganizationDto>> GetPaginatedOrganizationsAsync(int page, int pageSize, string search = null);
        Task FollowOrganizationAsync(Guid organizationId);
        Task AcceptFollowRequestAsync(Guid organizationId, Guid requestId);
        Task RejectFollowRequestAsync(Guid organizationId, Guid requestId, string reason);
        Task<IEnumerable<OrganizationGalleryUsersDto>> GetUserFollowedOrganizationsAsync(Guid userId);
        Task<PagedResult<OrganizationRequestDto>> GetOrganizationRequestsAsync(Guid organizationId, int page, int pageSize);
    }
}
