using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
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
        private readonly IMongoRepository<UserRequestsDocument, Guid> _studentRequestsRepository;

        public GetSentFriendRequestsHandler(IMongoRepository<UserRequestsDocument, Guid> studentRequestsRepository)
        {
            _studentRequestsRepository = studentRequestsRepository;
        }

       public async Task<IEnumerable<UserRequestsDto>> HandleAsync(GetSentFriendRequests query, CancellationToken cancellationToken)
        {
            var studentRequests = await _studentRequestsRepository.Collection
                .Find(doc => doc.UserId == query.UserId)
                .ToListAsync(cancellationToken);

            if (studentRequests == null || !studentRequests.Any())
            {
                return Enumerable.Empty<UserRequestsDto>();
            }

            var sentRequests = studentRequests
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
                            State = request.State, 
                            UserId = request.InviterId
                        })
                        .ToList()
                })
                .Where(dto => dto.FriendRequests.Any())
                .ToList();

            return sentRequests;
        }
    }
}
