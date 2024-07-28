using MiniSpace.Services.Organizations.Core.Entities;
using System;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Repositories
{
    public interface IOrganizationMembersRepository
    {
        Task<User> GetMemberAsync(Guid organizationId, Guid memberId);
        Task<IEnumerable<User>> GetMembersAsync(Guid organizationId);
        Task AddMemberAsync(User member);
        Task UpdateMemberAsync(User member);
        Task DeleteMemberAsync(Guid organizationId, Guid memberId);
    }
}
