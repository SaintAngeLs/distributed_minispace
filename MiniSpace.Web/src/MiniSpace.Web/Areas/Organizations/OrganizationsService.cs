using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
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
        
        public Task<OrganizationDto> GetOrganizationAsync(Guid organizationId, Guid rootId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<OrganizationDto>($"organizations/{organizationId}?rootId={rootId}");
        }

        public Task<OrganizationDetailsDto> GetOrganizationDetailsAsync(Guid organizationId, Guid rootId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<OrganizationDetailsDto>($"organizations/{organizationId}/details?rootId={rootId}");
        }

        public Task<IEnumerable<OrganizationDto>> GetOrganizerOrganizationsAsync(Guid organizerId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<OrganizationDto>>($"organizations/organizer/{organizerId}");
        }

        public Task<IEnumerable<OrganizationDto>> GetRootOrganizationsAsync()
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<OrganizationDto>>("organizations/root");
        }

        public Task<IEnumerable<OrganizationDto>> GetChildrenOrganizationsAsync(Guid organizationId, Guid rootId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<OrganizationDto>>
                ($"organizations/{organizationId}/children?rootId={rootId}");
        }

        public Task DeleteOrganizationAsync(Guid organizationId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"organizations/{organizationId}");
        }

        public Task<IEnumerable<Guid>> GetAllChildrenOrganizationsAsync(Guid organizationId, Guid rootId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<Guid>>
                ($"organizations/{organizationId}/children/all?rootId={rootId}");
        }
        
        public Task<HttpResponse<object>> CreateOrganizationAsync(Guid organizationId, string name, Guid rootId, Guid parentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>($"organizations/{organizationId}/children",
                new {organizationId, name, rootId, parentId});
        }

        public Task<HttpResponse<object>> CreateRootOrganizationAsync(Guid organizationId, string name)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>("organizations", new {organizationId, name});
        }
        
        public Task AddOrganizerToOrganizationAsync(Guid rootOrganizationId, Guid organizationId, Guid organizerId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync($"organizations/{organizationId}/organizer",
                new {rootOrganizationId, organizationId, organizerId});
        }

        public Task RemoveOrganizerFromOrganizationAsync(Guid rootOrganizationId, Guid organizationId, Guid organizerId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"organizations/{organizationId}/organizer/{organizerId}?rootOrganizationId={rootOrganizationId}");
        }
    }    
}
