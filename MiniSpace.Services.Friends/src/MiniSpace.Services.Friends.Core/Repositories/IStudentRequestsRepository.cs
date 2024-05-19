using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Repositories
{
    public interface IStudentRequestsRepository
    {
        Task<StudentRequests> GetAsync(Guid studentId);
        Task<IEnumerable<StudentRequests>> GetAllAsync();
        Task AddAsync(StudentRequests studentRequests);
        Task UpdateAsync(StudentRequests studentRequests);
        Task UpdateAsync(Guid studentId, IEnumerable<FriendRequest> updatedFriendRequests);
        Task DeleteAsync(Guid studentId);
    }
}
