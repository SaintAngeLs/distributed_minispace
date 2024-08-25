using Convey.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Core.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Queries.Handlers
{
    public class GetFriendsHandler : IQueryHandler<GetFriends, PagedResponse<UserFriendsDto>>
    {
        private readonly IUserFriendsRepository _userFriendsRepository;

        public GetFriendsHandler(IUserFriendsRepository userFriendsRepository)
        {
            _userFriendsRepository = userFriendsRepository;
        }

        public async Task<PagedResponse<UserFriendsDto>> HandleAsync(GetFriends query, CancellationToken cancellationToken)
        {
            var allFriends = await _userFriendsRepository.GetFriendsAsync(query.UserId);

            if (allFriends == null || !allFriends.Any())
            {
                return new PagedResponse<UserFriendsDto>(Enumerable.Empty<UserFriendsDto>(), query.Page, query.PageSize, 0);
            }

            var totalItems = allFriends.Count();

            var friendsToReturn = allFriends
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(f => new FriendDto
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    FriendId = f.FriendId,
                    CreatedAt = f.CreatedAt,
                    State = f.FriendState
                })
                .ToList();

            var userFriendsDto = new UserFriendsDto
            {
                UserId = query.UserId,
                Friends = friendsToReturn
            };

            var userFriendsDtos = new List<UserFriendsDto> { userFriendsDto };

            return new PagedResponse<UserFriendsDto>(userFriendsDtos, query.Page, query.PageSize, totalItems);
        }
    }
}
