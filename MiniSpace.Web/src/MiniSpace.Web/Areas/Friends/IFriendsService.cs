using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Friends
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
        Task<PaginatedResponseDto<StudentDto>> GetAllStudentsAsync(int page = 1, int resultsPerPage = 10);
        Task InviteStudent(Guid inviterId, Guid inviteeId);
         Task<IEnumerable<FriendRequestDto>> GetSentFriendRequestsAsync();
        Task<IEnumerable<FriendRequestDto>> GetIncomingFriendRequestsAsync();
        Task AcceptFriendRequestAsync(Guid requestId, Guid requesterId, Guid friendId);
        Task DeclineFriendRequestAsync(Guid requestId, Guid requesterId, Guid friendId);
    }
}
