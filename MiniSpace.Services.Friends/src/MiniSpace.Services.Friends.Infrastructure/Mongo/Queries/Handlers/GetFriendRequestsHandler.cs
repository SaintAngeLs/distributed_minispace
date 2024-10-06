using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Core.Entities;
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
    public class GetFriendRequestsHandler : IQueryHandler<GetFriendRequests, PagedResponse<FriendRequestDto>>
    {
        private readonly IMongoRepository<FriendRequestDocument, Guid> _friendRequestRepository;

        public GetFriendRequestsHandler(IMongoRepository<FriendRequestDocument, Guid> friendRequestRepository)
        {
            _friendRequestRepository = friendRequestRepository;
        }

        public async Task<PagedResponse<FriendRequestDto>> HandleAsync(GetFriendRequests query, CancellationToken cancellationToken)
        {
            var filter = Builders<FriendRequestDocument>.Filter.And(
                Builders<FriendRequestDocument>.Filter.Eq(fr => fr.InviteeId, query.UserId),
                Builders<FriendRequestDocument>.Filter.Eq(fr => fr.State, FriendState.Requested)
            );

            var totalItems = (int)await _friendRequestRepository.Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            var documents = await _friendRequestRepository.Collection
                .Find(filter)
                .Skip((query.Page - 1) * query.PageSize)
                .Limit(query.PageSize)
                .ToListAsync(cancellationToken);

            var friendRequests = documents.Select(doc => doc.AsDto());

            return new PagedResponse<FriendRequestDto>(friendRequests, query.Page, query.PageSize, totalItems);
        }
    }
}
