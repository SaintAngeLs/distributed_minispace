using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
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
    public class GetSentFriendRequestsHandler : IQueryHandler<GetSentFriendRequests, IEnumerable<UserRequestsDto>>
    {
        private readonly IMongoRepository<UserRequestsDocument, Guid> _userRequestsRepository;

        public GetSentFriendRequestsHandler(IMongoRepository<UserRequestsDocument, Guid> userRequestsRepository)
        {
            _userRequestsRepository = userRequestsRepository;
        }

        public async Task<IEnumerable<UserRequestsDto>> HandleAsync(GetSentFriendRequests query, CancellationToken cancellationToken)
        {
            var userRequests = await _userRequestsRepository.Collection
                .Find(doc => doc.UserId == query.UserId)
                .ToListAsync(cancellationToken);

            if (userRequests == null || !userRequests.Any())
            {
                return Enumerable.Empty<UserRequestsDto>();
            }

            var sentRequests = userRequests
                .Select(doc => new UserRequestsDto
                {
                    Id = doc.Id,
                    UserId = doc.UserId,
                    FriendRequests = doc.FriendRequests
                        .Where(request => request.InviterId == query.UserId && request.State == Core.Entities.FriendState.Requested)
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

            return sentRequests;
        }
    }
}
