using MiniSpace.Services.Organizations.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Repositories
{
    public interface IOrganizationRolesRepository
    {
        Task<Role> GetRoleAsync(Guid organizationId, Guid roleId);
        Task<IEnumerable<Role>> GetRolesAsync(Guid organizationId);
        Task<Role> GetRoleByNameAsync(string roleName);
        Task AddRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(Guid roleId);
    }
}
