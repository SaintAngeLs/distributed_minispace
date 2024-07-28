using MiniSpace.Services.Organizations.Core.Entities;
using System;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Repositories
{
    public interface IOrganizationSettingsRepository
    {
        Task<OrganizationSettings> GetAsync(Guid organizationId);
        Task UpdateAsync(OrganizationSettings settings);
    }
}
