using System;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Dto.Organizations;

namespace MiniSpace.Services.Notifications.Application.Services.Clients
{
    public interface IOrganizationsServiceClient
    {
        Task<OrganizationDto> GetOrganizationAsync(Guid organizationId);
        Task<OrganizationUsersDto> GetOrganizationMembersAsync(Guid organizationId);
    }
}
