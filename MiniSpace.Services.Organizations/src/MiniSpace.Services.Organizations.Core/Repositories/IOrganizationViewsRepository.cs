using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Core.Repositories
{
    public interface IOrganizationViewsRepository
    {
        Task<OrganizationViews> GetAsync(Guid organizationId);
        Task AddAsync(OrganizationViews organizationViews);
        Task UpdateAsync(OrganizationViews organizationViews);
        Task DeleteAsync(Guid organizationId);
    }
}
