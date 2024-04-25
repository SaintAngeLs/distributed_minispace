using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;

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
            var documents = await _friendRequestRepository.FindAsync(p => p.InviteeId == query.StudentId && p.State == FriendState.Requested);

            return documents.Select(doc => doc.AsDto());
        }
    }
}
