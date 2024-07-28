using MiniSpace.Services.Organizations.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Repositories
{
    public interface IUserInvitationsRepository
    {
        Task<Invitation> GetInvitationAsync(Guid organizationId, Guid userId);
        Task<IEnumerable<Invitation>> GetInvitationsAsync(Guid organizationId);
        Task AddInvitationAsync(Invitation invitation);
        Task DeleteInvitationAsync(Guid organizationId, Guid userId);
    }
}
