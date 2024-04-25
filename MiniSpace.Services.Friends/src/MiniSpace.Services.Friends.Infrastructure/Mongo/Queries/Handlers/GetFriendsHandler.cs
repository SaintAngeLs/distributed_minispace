using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Queries.Handlers
{
    public class GetFriendsHandler : IQueryHandler<GetFriends, IEnumerable<FriendDto>>
    {
        private readonly IMongoRepository<FriendDocument, Guid> _friendRepository;

        public GetFriendsHandler(IMongoRepository<FriendDocument, Guid> friendRepository)
        {
            _friendRepository = friendRepository;
        }

        public async Task<IEnumerable<FriendDto>> HandleAsync(GetFriends query, CancellationToken cancellationToken)
        {
            var documents = await _friendRepository.FindAsync(p => p.StudentId == query.StudentId);

            return documents.Select(doc => doc.AsDto());
        }
    }
}
