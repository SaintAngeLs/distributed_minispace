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
            var documents = await _friendRepository.FindAsync(
                doc => doc.StudentId == query.StudentId || doc.FriendId == query.StudentId);

            return documents.Select(doc => 
            {
                if (doc.StudentId == query.StudentId) {
                    return doc.AsDto();
                } else {
                    return new FriendDto {
                        Id = doc.Id,
                        StudentId = doc.FriendId, 
                        FriendId = doc.StudentId,
                        CreatedAt = doc.CreatedAt,
                        State = doc.State,
                    };
                }
            });
        }
    }
}
