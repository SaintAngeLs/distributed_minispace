using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using System.Text.Json;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Queries.Handlers
{
    public class GetFriendRequestsHandler : IQueryHandler<GetFriendRequests, IEnumerable<FriendRequestDto>>
    {
        private readonly IMongoRepository<FriendRequestDocument, Guid> _friendRequestRepository;
        

        public GetFriendRequestsHandler(IMongoRepository<FriendRequestDocument, Guid> friendRequestRepository)
        {
            _friendRequestRepository = friendRequestRepository;
        }

        public async Task<IEnumerable<FriendRequestDto>> HandleAsync(GetFriendRequests query, CancellationToken cancellationToken)
        {
            string queryJson = JsonSerializer.Serialize(query);
            Console.WriteLine($"Handling GetFriendRequests: {queryJson}");
            Console.WriteLine($"Handling GetFriendRequests for UserId: {query.StudentId}");

            var documents = await _friendRequestRepository.FindAsync(p => p.InviteeId == query.StudentId && p.State == FriendState.Requested);
            Console.WriteLine($"Found {documents.Count()} friend requests.");

            if (!documents.Any())
            {
                Console.WriteLine($"No friend requests found for UserId: {query.StudentId}.");
                return Enumerable.Empty<FriendRequestDto>();
            }

            return documents.Select(doc => doc.AsDto());
        }

    }
}
