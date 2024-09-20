using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Core.Repositories
{
    public interface IUserOrganizationViewsRepository
    {
        Task<UserOrganizationsViews> GetAsync(Guid userId);
        Task AddAsync(UserOrganizationsViews userOrganizationsViews);
        Task UpdateAsync(UserOrganizationsViews userOrganizationsViews);
        Task DeleteAsync(Guid userId);
    }
}
