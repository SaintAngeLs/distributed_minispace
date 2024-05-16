using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Queries.Handlers
{
    public class GetSentFriendRequestsHandler : IQueryHandler<GetSentFriendRequests, IEnumerable<FriendRequestDto>>
    {
        private readonly IMongoRepository<FriendRequestDocument, Guid> _friendRequestRepository;

        public GetSentFriendRequestsHandler(IMongoRepository<FriendRequestDocument, Guid> friendRequestRepository)
        {
            _friendRequestRepository = friendRequestRepository;
        }

        public async Task<IEnumerable<FriendRequestDto>> HandleAsync(GetSentFriendRequests query, CancellationToken cancellationToken)
        {
            // Console.WriteLine($"Fetching sent friend requests for student ID: {query.StudentId}");

            var documents = await _friendRequestRepository.FindAsync(
                doc => doc.InviterId == query.StudentId
              
            );

            if (!documents.Any()) {
                Console.WriteLine("No documents found");
                return Enumerable.Empty<FriendRequestDto>();
            }

            return documents.Select(doc => new FriendRequestDto
            {
                Id = doc.Id,
                InviterId = doc.InviterId,
                InviteeId = doc.InviteeId,
                RequestedAt = doc.RequestedAt,
                State = doc.State
            });
        }


    }
}
