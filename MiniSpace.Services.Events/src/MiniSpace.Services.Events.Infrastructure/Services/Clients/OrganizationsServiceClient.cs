using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Convey.HTTP;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Services.Clients;

namespace MiniSpace.Services.Events.Infrastructure.Services.Clients
{
    [ExcludeFromCodeCoverage]
    public class OrganizationsServiceClient : IOrganizationsServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;
        
        public OrganizationsServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["organizations"];
        }
        
        public Task<OrganizationDto> GetAsync(Guid organizationId, Guid rootId)
            => _httpClient.GetAsync<OrganizationDto>($"{_url}/organizations/{organizationId}/details?rootId={rootId}");

        public Task<IEnumerable<Guid>> GetAllChildrenOrganizations(Guid organizationId, Guid rootId)
            => _httpClient.GetAsync<IEnumerable<Guid>>($"{_url}/organizations/{organizationId}/children/all?rootId={rootId}");

    }
}