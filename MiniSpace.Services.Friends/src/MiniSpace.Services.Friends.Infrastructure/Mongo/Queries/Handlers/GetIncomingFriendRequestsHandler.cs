using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
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
    public class GetIncomingFriendRequestsHandler : IQueryHandler<GetIncomingFriendRequests, PagedResponse<UserRequestsDto>>
    {
        private readonly IMongoRepository<UserRequestsDocument, Guid> _userRequestsRepository;

        public GetIncomingFriendRequestsHandler(IMongoRepository<UserRequestsDocument, Guid> userRequestsRepository)
        {
            _userRequestsRepository = userRequestsRepository;
        }

        public async Task<PagedResponse<UserRequestsDto>> HandleAsync(GetIncomingFriendRequests query, CancellationToken cancellationToken)
        {
            var filter = Builders<UserRequestsDocument>.Filter.Eq(doc => doc.UserId, query.UserId);
            var totalItems = (int)await _userRequestsRepository.Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            var userRequests = await _userRequestsRepository.Collection
                .Find(filter)
                .Skip((query.Page - 1) * query.PageSize)
                .Limit(query.PageSize)
                .ToListAsync(cancellationToken);

            if (userRequests == null || !userRequests.Any())
            {
                return new PagedResponse<UserRequestsDto>(Enumerable.Empty<UserRequestsDto>(), query.Page, query.PageSize, 0);
            }

            var incomingRequests = userRequests
                .Select(doc => new UserRequestsDto
                {
                    Id = doc.Id,
                    UserId = doc.UserId,
                    FriendRequests = doc.FriendRequests
                        .Where(request => request.InviteeId == query.UserId && request.State != Core.Entities.FriendState.Accepted)
                        .Select(request => new FriendRequestDto
                        {
                            Id = request.Id,
                            InviterId = request.InviterId,
                            InviteeId = request.InviteeId,
                            RequestedAt = request.RequestedAt,
                            State = request.State
                        })
                        .ToList()
                })
                .Where(dto => dto.FriendRequests.Any())
                .ToList();

            return new PagedResponse<UserRequestsDto>(incomingRequests, query.Page, query.PageSize, totalItems);
        }
    }
}
