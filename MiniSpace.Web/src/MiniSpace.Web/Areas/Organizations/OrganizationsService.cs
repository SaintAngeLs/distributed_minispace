using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.Areas.Organizations.CommandsDto;
using MiniSpace.Web.DTO.Organizations;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Organizations
{
    public class OrganizationsService : IOrganizationsService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;
        
        public OrganizationsService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }

        public Task<OrganizationDto> GetOrganizationAsync(Guid organizationId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<OrganizationDto>($"organizations/{organizationId}");
        }

        public Task<OrganizationDetailsDto> GetOrganizationDetailsAsync(Guid organizationId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<OrganizationDetailsDto>($"organizations/{organizationId}/details");
        }

        public Task<IEnumerable<OrganizationDto>> GetRootOrganizationsAsync()
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<OrganizationDto>>("organizations/root");
        }

        public Task<IEnumerable<OrganizationDto>> GetChildrenOrganizationsAsync(Guid organizationId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<OrganizationDto>>($"organizations/{organizationId}/children");
        }

        public Task<IEnumerable<Guid>> GetAllChildrenOrganizationsAsync(Guid organizationId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<Guid>>($"organizations/{organizationId}/children/all");
        }

        public Task<OrganizationGalleryUsersDto> GetOrganizationWithGalleryAndUsersAsync(Guid organizationId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<OrganizationGalleryUsersDto>($"organizations/{organizationId}/details/gallery-users");
        }

        public Task<HttpResponse<object>> CreateOrganizationAsync(CreateOrganizationDto command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<CreateOrganizationDto, object>("organizations", command);
        }

        public Task<HttpResponse<object>> CreateSubOrganizationAsync(Guid parentOrganizationId, CreateSubOrganizationCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<CreateSubOrganizationCommand, object>($"organizations/{parentOrganizationId}/children", command);
        }

        public Task DeleteOrganizationAsync(Guid organizationId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"organizations/{organizationId}");
        }

        public Task<HttpResponse<object>> CreateOrganizationRoleAsync(Guid organizationId, CreateOrganizationRoleCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<CreateOrganizationRoleCommand, object>($"organizations/{organizationId}/roles", command);
        }

        public Task InviteUserToOrganizationAsync(Guid organizationId, InviteUserToOrganizationCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync($"organizations/{organizationId}/invite", command);
        }

        public Task AssignRoleToMemberAsync(Guid organizationId, Guid memberId, AssignRoleToMemberCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync($"organizations/{organizationId}/roles/{memberId}", command);
        }

        public Task<HttpResponse<object>> UpdateRolePermissionsAsync(Guid organizationId, Guid roleId, UpdateRolePermissionsCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync<UpdateRolePermissionsCommand, object>($"organizations/{organizationId}/roles/{roleId}/permissions", command);
        }

        public Task SetOrganizationPrivacyAsync(Guid organizationId, SetOrganizationPrivacyCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync($"organizations/{organizationId}/privacy", command);
        }

        public Task<HttpResponse<object>> UpdateOrganizationSettingsAsync(Guid organizationId, UpdateOrganizationSettingsCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync<UpdateOrganizationSettingsCommand, object>($"organizations/{organizationId}/settings", command);
        }

        public Task SetOrganizationVisibilityAsync(Guid organizationId, SetOrganizationVisibilityCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync($"organizations/{organizationId}/visibility", command);
        }

        public Task ManageFeedAsync(Guid organizationId, ManageFeedCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync($"organizations/{organizationId}/feed", command);
        }

        public Task<HttpResponse<object>> UpdateOrganizationAsync(Guid organizationId, UpdateOrganizationCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync<UpdateOrganizationCommand, object>($"organizations/{organizationId}", command);
        }

        public Task<IEnumerable<OrganizationDto>> GetUserOrganizationsAsync(Guid userId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<OrganizationDto>>($"organizations/users/{userId}/organizations");
        }

        public Task<IEnumerable<RoleDto>> GetOrganizationRolesAsync(Guid organizationId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<RoleDto>>($"organizations/{organizationId}/roles");
        }
    }
}
