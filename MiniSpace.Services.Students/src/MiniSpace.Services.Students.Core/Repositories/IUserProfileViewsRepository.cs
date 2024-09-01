using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Repositories
{
    public interface IUserProfileViewsRepository
    {
        Task<UserProfileViews> GetAsync(Guid userId);
        Task AddAsync(UserProfileViews userProfileViews);
        Task UpdateAsync(UserProfileViews userProfileViews);
        Task DeleteAsync(Guid userId);
    }
}
