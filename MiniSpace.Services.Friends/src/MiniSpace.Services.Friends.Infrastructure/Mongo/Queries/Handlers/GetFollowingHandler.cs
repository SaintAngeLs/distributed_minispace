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
    public class GetFollowingHandler : IQueryHandler<GetFollowing, PagedResponse<UserFriendsDto>>
    {
        private readonly IUserFriendsRepository _userFriendsRepository;
        private readonly IMongoRepository<UserRequestsDocument, Guid> _userRequestsRepository;

        public GetFollowingHandler(IUserFriendsRepository userFriendsRepository, IMongoRepository<UserRequestsDocument, Guid> userRequestsRepository)
        {
            _userFriendsRepository = userFriendsRepository;
            _userRequestsRepository = userRequestsRepository;
        }

        public async Task<PagedResponse<UserFriendsDto>> HandleAsync(GetFollowing query, CancellationToken cancellationToken)
        {
            var friends = await _userFriendsRepository.GetFriendsAsync(query.UserId);

            var sentRequestsFilter = Builders<UserRequestsDocument>.Filter.Eq(doc => doc.UserId, query.UserId);
            var sentRequests = await _userRequestsRepository.Collection
                .Find(sentRequestsFilter)
                .ToListAsync(cancellationToken);

            var following = friends.Select(f => new FriendDto
            {
                Id = f.Id,
                UserId = f.UserId,
                FriendId = f.FriendId,
                CreatedAt = f.CreatedAt,
                State = f.FriendState
            }).ToList();

            var sentRequestDtos = sentRequests.SelectMany(doc => doc.FriendRequests
                .Where(request => request.InviterId == query.UserId && request.State == Core.Entities.FriendState.Requested)
                .Select(request => new FriendDto
                {
                    Id = request.Id,
                    UserId = request.InviterId,
                    FriendId = request.InviteeId,  // The invitee is the one being followed
                    CreatedAt = request.RequestedAt,
                    State = request.State
                })).ToList();

            following.AddRange(sentRequestDtos);

            var totalItems = following.Count;
            var paginatedFollowing = following
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            var response = new UserFriendsDto
            {
                UserId = query.UserId,
                Friends = paginatedFollowing
            };

            return new PagedResponse<UserFriendsDto>(new List<UserFriendsDto> { response }, query.Page, query.PageSize, totalItems);
        }
    }
}
