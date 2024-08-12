using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Repositories
{
    public interface IUserOrganizationsRepository
    {
        Task<IEnumerable<Guid>> GetUserOrganizationsAsync(Guid userId);
        Task AddOrganizationToUserAsync(Guid userId, Guid organizationId);
        Task RemoveOrganizationFromUserAsync(Guid userId, Guid organizationId);
        Task<bool> IsUserInOrganizationAsync(Guid userId, Guid organizationId);
    }
}
