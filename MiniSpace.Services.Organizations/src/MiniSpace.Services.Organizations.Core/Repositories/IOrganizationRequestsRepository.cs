using MiniSpace.Services.Organizations.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Repositories
{
    public interface IOrganizationRequestsRepository
    {
        Task<OrganizationRequest> GetRequestAsync(Guid organizationId, Guid requestId);
        Task<IEnumerable<OrganizationRequest>> GetRequestsAsync(Guid organizationId);
        Task AddRequestAsync(Guid organizationId, OrganizationRequest request);
        Task UpdateRequestAsync(Guid organizationId, OrganizationRequest request);
        Task DeleteRequestAsync(Guid organizationId, Guid requestId);
    }
}
