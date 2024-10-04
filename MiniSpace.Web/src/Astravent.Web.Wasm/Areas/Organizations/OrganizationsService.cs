using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.Areas.Identity;
using Astravent.Web.Wasm.Areas.Organizations.CommandsDto;
using Astravent.Web.Wasm.Areas.PagedResult;
using Astravent.Web.Wasm.DTO.Organizations;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Organizations
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

        public async Task<OrganizationDto> GetOrganizationAsync(Guid organizationId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<OrganizationDto>($"organizations/{organizationId}");
        }

        public async Task<OrganizationDetailsDto> GetOrganizationDetailsAsync(Guid organizationId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<OrganizationDetailsDto>($"organizations/{organizationId}/details");
        }

        public async Task<IEnumerable<OrganizationDto>> GetRootOrganizationsAsync()
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<IEnumerable<OrganizationDto>>("organizations/root");
        }

        public async Task<PagedResult<OrganizationDto>> GetChildrenOrganizationsAsync(Guid organizationId, int page, int pageSize)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var queryString = $"organizations/{organizationId}/children?page={page}&pageSize={pageSize}";
            return await _httpClient.GetAsync<PagedResult<OrganizationDto>>(queryString);
        }


        public async Task<IEnumerable<Guid>> GetAllChildrenOrganizationsAsync(Guid organizationId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<IEnumerable<Guid>>($"organizations/{organizationId}/children/all");
        }

        public async Task<OrganizationGalleryUsersDto> GetOrganizationWithGalleryAndUsersAsync(Guid organizationId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<OrganizationGalleryUsersDto>($"organizations/{organizationId}/details/gallery-users");
        }

        public async Task<HttpResponse<object>> CreateOrganizationAsync(CreateOrganizationDto command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<CreateOrganizationDto, object>("organizations", command);
        }

        public async Task<HttpResponse<object>> CreateSubOrganizationAsync(Guid parentOrganizationId, CreateSubOrganizationCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<CreateSubOrganizationCommand, object>($"organizations/{parentOrganizationId}/children", command);
        }

        public async Task DeleteOrganizationAsync(Guid organizationId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.DeleteAsync($"organizations/{organizationId}");
        }

        public async Task<HttpResponse<object>> CreateOrganizationRoleAsync(Guid organizationId, CreateOrganizationRoleCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<CreateOrganizationRoleCommand, object>($"organizations/{organizationId}/roles", command);
        }

        public async Task InviteUserToOrganizationAsync(Guid organizationId, InviteUserToOrganizationCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PostAsync($"organizations/{organizationId}/invite", command);
        }

        public async Task AssignRoleToMemberAsync(Guid organizationId, Guid memberId, AssignRoleToMemberCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PostAsync($"organizations/{organizationId}/roles/{memberId}", command);
        }

        public async Task<HttpResponse<object>> UpdateRolePermissionsAsync(Guid organizationId, Guid roleId, UpdateRolePermissionsCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PutAsync<UpdateRolePermissionsCommand, object>($"organizations/{organizationId}/roles/{roleId}/permissions", command);
        }

        public async Task SetOrganizationPrivacyAsync(Guid organizationId, SetOrganizationPrivacyCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PostAsync($"organizations/{organizationId}/privacy", command);
        }

        public async Task<HttpResponse<object>> UpdateOrganizationSettingsAsync(Guid organizationId, UpdateOrganizationSettingsCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PutAsync<UpdateOrganizationSettingsCommand, object>($"organizations/{organizationId}/settings", command);
        }

        public async Task SetOrganizationVisibilityAsync(Guid organizationId, SetOrganizationVisibilityCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PutAsync($"organizations/{organizationId}/visibility", command);
        }

        public async Task ManageFeedAsync(Guid organizationId, ManageFeedCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PutAsync($"organizations/{organizationId}/feed", command);
        }

        public async Task<HttpResponse<object>> UpdateOrganizationAsync(Guid organizationId, UpdateOrganizationCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PutAsync<UpdateOrganizationCommand, object>($"organizations/{organizationId}", command);
        }

        public async Task<PagedResult<OrganizationDto>> GetPaginatedUserOrganizationsAsync(Guid userId, int page, int pageSize)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var queryString = $"organizations/users/{userId}/organizations?page={page}&pageSize={pageSize}";
            return await _httpClient.GetAsync<PagedResult<OrganizationDto>>(queryString);
        }

        public async Task<IEnumerable<RoleDto>> GetOrganizationRolesAsync(Guid organizationId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<IEnumerable<RoleDto>>($"organizations/{organizationId}/roles");
        }

        public async Task<PagedResult<OrganizationDto>> GetPaginatedOrganizationsAsync(int page, int pageSize, string search = null)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var queryString = $"organizations/paginated?page={page}&pageSize={pageSize}&search={search}";
            return await _httpClient.GetAsync<PagedResult<OrganizationDto>>(queryString);
        }

        public async Task FollowOrganizationAsync(Guid organizationId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var command = new FollowOrganizationDto
            {
                UserId = _identityService.UserDto.Id,
                OrganizationId = organizationId
            };
            await _httpClient.PostAsync($"organizations/{organizationId}/follow", command);
        }

        public async Task AcceptFollowRequestAsync(Guid organizationId, Guid requestId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var command = new AcceptFollowRequestDto
            {
                OrganizationId = organizationId,
                RequestId = requestId
            };
            await _httpClient.PutAsync($"organizations/{organizationId}/requests/{requestId}/accept", command);
        }

        public async Task RejectFollowRequestAsync(Guid organizationId, Guid requestId, string reason)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var command = new RejectFollowRequestDto
            {
                OrganizationId = organizationId,
                RequestId = requestId,
                Reason = reason
            };
            await _httpClient.PutAsync($"organizations/{organizationId}/requests/{requestId}/reject", command);
        }

        public async Task<IEnumerable<OrganizationGalleryUsersDto>> GetUserFollowedOrganizationsAsync(Guid userId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<IEnumerable<OrganizationGalleryUsersDto>>($"organizations/users/{userId}/organizations/follow");
        }

        public async Task<PagedResult<OrganizationRequestDto>> GetOrganizationRequestsAsync(Guid organizationId, int page, int pageSize)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var queryString = $"organizations/{organizationId}/requests?page={page}&pageSize={pageSize}";
            return await _httpClient.GetAsync<PagedResult<OrganizationRequestDto>>(queryString);
        }

    }
}
