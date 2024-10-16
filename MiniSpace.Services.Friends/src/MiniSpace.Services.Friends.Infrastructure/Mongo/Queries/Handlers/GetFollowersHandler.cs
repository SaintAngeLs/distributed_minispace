using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Core.Wrappers;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Queries.Handlers
{
    public class GetFollowersHandler : IQueryHandler<GetFollowers, PagedResponse<UserFriendsDto>>
    {
        private readonly IUserFriendsRepository _userFriendsRepository;
        private readonly IMongoRepository<UserRequestsDocument, Guid> _userRequestsRepository;

        public GetFollowersHandler(IUserFriendsRepository userFriendsRepository, IMongoRepository<UserRequestsDocument, Guid> userRequestsRepository)
        {
            _userFriendsRepository = userFriendsRepository;
            _userRequestsRepository = userRequestsRepository;
        }

        public async Task<PagedResponse<UserFriendsDto>> HandleAsync(GetFollowers query, CancellationToken cancellationToken)
        {
            // Step 1: Get all users who have sent friend requests to the user (followers)
            var incomingRequestsFilter = Builders<UserRequestsDocument>.Filter.Eq(doc => doc.UserId, query.UserId);
            var incomingRequests = await _userRequestsRepository.Collection
                .Find(incomingRequestsFilter)
                .ToListAsync(cancellationToken);

            var followersFromRequests = incomingRequests
                .SelectMany(doc => doc.FriendRequests
                    .Where(request => request.InviteeId == query.UserId && request.State == Core.Entities.FriendState.Requested)
                    .Select(request => new FriendDto
                    {
                        Id = request.Id,
                        UserId = request.InviterId,  
                        FriendId = request.InviteeId,
                        CreatedAt = request.RequestedAt,
                        State = request.State
                    }))
                .ToList();

            // Step 2: Get all friends of the user (friends are also followers)
            var friends = await _userFriendsRepository.GetFriendsAsync(query.UserId);

            var followersFromFriends = friends.Select(f => new FriendDto
            {
                Id = f.Id,
                UserId = f.FriendId,
                FriendId = f.UserId,
                CreatedAt = f.CreatedAt,
                State = f.FriendState
            }).ToList();

            // Step 3: Combine followers from requests and friends
            var allFollowers = followersFromRequests.Concat(followersFromFriends).Distinct().ToList();

            // Step 4: Paginate the combined followers list
            var totalItems = allFollowers.Count;
            var paginatedFollowers = allFollowers
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            var response = new UserFriendsDto
            {
                UserId = query.UserId,
                Friends = paginatedFollowers
            };

            return new PagedResponse<UserFriendsDto>(new List<UserFriendsDto> { response }, query.Page, query.PageSize, totalItems);
        }
    }
}
