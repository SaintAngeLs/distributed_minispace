using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Repositories
{
    public interface IStudentFriendsRepository
    {
        Task<StudentFriends> GetAsync(Guid studentId);
        Task<IEnumerable<StudentFriends>> GetAllAsync();
        Task AddAsync(StudentFriends studentFriends);
        Task UpdateAsync(StudentFriends studentFriends);
        Task DeleteAsync(Guid studentId);
        Task<bool> ExistsAsync(Guid studentId);
        Task<IEnumerable<Friend>> GetFriendsAsync(Guid studentId);
        Task AddOrUpdateAsync(StudentFriends studentFriends);
    }
}
