using MiniSpace.Services.Organizations.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Repositories
{
    public interface IOrganizationRepository
    {
        Task<Organization> GetAsync(Guid id);
        Task<IEnumerable<Organization>> GetOrganizerOrganizationsAsync(Guid organizerId);
        Task AddAsync(Organization organization);
        Task UpdateAsync(Organization organization);
        Task DeleteAsync(Guid id);

        Task<User> GetMemberAsync(Guid organizationId, Guid memberId);
        Task<IEnumerable<User>> GetMembersAsync(Guid organizationId);
        Task AddMemberAsync(Guid organizationId, User member);
        Task UpdateMemberAsync(Guid organizationId, User member);
        Task DeleteMemberAsync(Guid organizationId, Guid memberId);


        Task<IEnumerable<Organization>> GetOrganizationsByUserAsync(Guid userId);
        Task AddSubOrganizationsAsync(Guid parentId, List<Organization> organizations);
    }
}
