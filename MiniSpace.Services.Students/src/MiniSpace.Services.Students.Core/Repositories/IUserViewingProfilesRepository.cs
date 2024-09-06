using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Repositories
{
    public interface IUserViewingProfilesRepository
    {
        Task<UserViewingProfiles> GetAsync(Guid userId);
        Task AddAsync(UserViewingProfiles userViewingProfiles);
        Task UpdateAsync(UserViewingProfiles userViewingProfiles);
        Task DeleteAsync(Guid userId);
    }
}
