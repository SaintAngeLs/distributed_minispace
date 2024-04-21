using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Repositories
{
    public interface IFriendRepository
    {
        Task<List<Student>> GetFriendsAsync(Guid studentId);
        Task AddFriendAsync(Guid requesterId, Guid friendId);
        Task RemoveFriendAsync(Guid requesterId, Guid friendId);
        Task<bool> IsFriendAsync(Guid studentId, Guid potentialFriendId);
    }
}
