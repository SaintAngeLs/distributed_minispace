using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpacePwa.DTO;
using MiniSpacePwa.HttpClients;

namespace MiniSpacePwa.Areas.Friends
{
    public interface IFriendsService
    {
        FriendDto FriendDto { get; }
        Task UpdateFriendDto(Guid friendId);
        void ClearFriendDto();
        Task<FriendDto> GetFriendAsync(Guid friendId);
        Task<HttpResponse<object>> AddFriendAsync(Guid friendId);
        Task RemoveFriendAsync(Guid friendId);
        Task<IEnumerable<FriendDto>> GetAllFriendsAsync(Guid studentId);
        Task<StudentDto> GetStudentAsync(Guid studentId);
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<PaginatedResponseDto<StudentDto>> GetAllStudentsAsync(int page = 1, int resultsPerPage = 10, string search = null);
        Task InviteStudent(Guid inviterId, Guid inviteeId);
        Task<IEnumerable<FriendRequestDto>> GetSentFriendRequestsAsync();
        Task<IEnumerable<FriendRequestDto>> GetIncomingFriendRequestsAsync();
        Task AcceptFriendRequestAsync(Guid requestId, Guid requesterId, Guid friendId);
        Task DeclineFriendRequestAsync(Guid requestId, Guid requesterId, Guid friendId);
        Task WithdrawFriendRequestAsync(Guid inviterId, Guid inviteeId);
    }
}
