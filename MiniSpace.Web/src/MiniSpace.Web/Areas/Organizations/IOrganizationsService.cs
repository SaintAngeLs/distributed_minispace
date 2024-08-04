using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Organizations.CommandsDto;
using MiniSpace.Web.DTO.Organizations;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Organizations
{
    public interface IOrganizationsService
    {
        Task<OrganizationDto> GetOrganizationAsync(Guid organizationId);
        Task<OrganizationDetailsDto> GetOrganizationDetailsAsync(Guid organizationId);
        Task<IEnumerable<OrganizationDto>> GetRootOrganizationsAsync();
        Task<IEnumerable<OrganizationDto>> GetChildrenOrganizationsAsync(Guid organizationId);
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
        Task<IEnumerable<OrganizationDto>> GetUserOrganizationsAsync(Guid userId);
        Task<IEnumerable<RoleDto>> GetOrganizationRolesAsync(Guid organizationId);
    }
}
