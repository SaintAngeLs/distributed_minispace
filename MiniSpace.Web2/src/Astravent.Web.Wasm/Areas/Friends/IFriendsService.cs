using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.Areas.Friends.CommandsDto;
using Astravent.Web.Wasm.Areas.PagedResult;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.DTO.Friends;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Friends
{
    public interface IFriendsService
    {
        FriendDto FriendDto { get; }

        Task UpdateFriendDto(Guid friendId);

        void ClearFriendDto();

        Task<FriendDto> GetFriendAsync(Guid friendId);

        Task<PagedResult<FriendDto>> GetAllFriendsAsync(Guid userId, int page = 1, int pageSize = 10);

        Task<PagedResult<FriendDto>> GetPagedFollowersAsync(Guid userId, int page = 1, int pageSize = 10);
        
        Task<PagedResult<FriendDto>> GetPagedFollowingAsync(Guid userId, int page = 1, int pageSize = 10);

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
