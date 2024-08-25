using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Friends.CommandsDto;
using MiniSpace.Web.Areas.PagedResult;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Friends;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Friends
{
    public interface IFriendsService
    {
        FriendDto FriendDto { get; }

        Task UpdateFriendDto(Guid friendId);

        void ClearFriendDto();

        Task<FriendDto> GetFriendAsync(Guid friendId);

        Task<PagedResult<FriendDto>> GetAllFriendsAsync(Guid userId, int page = 1, int pageSize = 10);

        Task<HttpResponse<object>> AddFriendAsync(Guid friendId);

        Task RemoveFriendAsync(Guid friendId);

        Task<StudentDto> GetStudentAsync(Guid studentId);
        
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();

        Task<PaginatedResponseDto<StudentDto>> GetAllStudentsAsync(int page = 1, int pageSize = 10, string searchTerm = null);

        Task InviteStudent(Guid inviterId, Guid inviteeId);

        Task<PagedResult<FriendRequestDto>> GetSentFriendRequestsAsync(int page = 1, int pageSize = 10);

        Task<PagedResult<FriendRequestDto>> GetIncomingFriendRequestsAsync(int page = 1, int pageSize = 10);

        Task AcceptFriendRequestAsync(FriendRequestActionDto requestAction);

        Task DeclineFriendRequestAsync(FriendRequestActionDto requestAction);

        Task WithdrawFriendRequestAsync(WithdrawFriendRequestDto withdrawRequest);
    }
}
