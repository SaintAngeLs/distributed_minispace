using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Data.Events;
using MiniSpace.Web.DTO.Wrappers;
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

        public Task<IEnumerable<OrganizationDto>> GetChildrenOrganizationsAsync(Guid organizationId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<OrganizationDto>>
                ($"organizations/{organizationId}/children?parentId={organizationId}");
        }

        public Task<HttpResponse<object>> AddOrganization(Guid organizationId, string name, Guid parentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>("organizations", new {organizationId, name, parentId});
        }
        
        public Task AddOrganizerToOrganization(Guid organizationId, Guid organizerId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync($"organizations/{organizationId}/organizer",
                new {organizationId, organizerId});
        }

        public Task RemoveOrganizerFromOrganization(Guid organizationId, Guid organizerId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"organizations/{organizationId}/organizer/{organizerId}");
        }
    }    
}
