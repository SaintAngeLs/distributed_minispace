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
    public class GetIncomingFriendRequestsHandler : IQueryHandler<GetIncomingFriendRequests, IEnumerable<StudentRequestsDto>>
    {
        private readonly IMongoRepository<StudentRequestsDocument, Guid> _studentRequestsRepository;

        public GetIncomingFriendRequestsHandler(IMongoRepository<StudentRequestsDocument, Guid> studentRequestsRepository)
        {
            _studentRequestsRepository = studentRequestsRepository;
        }

        public async Task<IEnumerable<StudentRequestsDto>> HandleAsync(GetIncomingFriendRequests query, CancellationToken cancellationToken)
        {
            var studentRequests = await _studentRequestsRepository.Collection
                .Find(doc => doc.StudentId == query.StudentId)
                .ToListAsync(cancellationToken);

            if (studentRequests == null || !studentRequests.Any())
            {
                return Enumerable.Empty<StudentRequestsDto>();
            }

            var incomingRequests = studentRequests
                .Select(doc => new StudentRequestsDto
                {
                    Id = doc.Id,
                    StudentId = doc.StudentId,
                    FriendRequests = doc.FriendRequests
                        .Where(request => request.InviteeId == query.StudentId && request.State != Core.Entities.FriendState.Accepted)
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

            return incomingRequests;
        }
    }
}
