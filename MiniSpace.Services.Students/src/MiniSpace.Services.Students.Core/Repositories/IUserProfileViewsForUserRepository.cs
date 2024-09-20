using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Repositories
{
    public interface IUserProfileViewsForUserRepository
    {
        Task<UserProfileViewsForUser> GetAsync(Guid userId);
        Task AddAsync(UserProfileViewsForUser userProfileViews);
        Task UpdateAsync(UserProfileViewsForUser userProfileViews);
        Task DeleteAsync(Guid userId);
    }
}
