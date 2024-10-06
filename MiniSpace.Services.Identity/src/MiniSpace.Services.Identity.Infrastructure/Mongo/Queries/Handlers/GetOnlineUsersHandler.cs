using System;
using System.Linq;
using System.Threading.Tasks;
using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Identity.Application.DTO;
using MiniSpace.Services.Identity.Application.Queries;
using MiniSpace.Services.Identity.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System.Threading;

namespace MiniSpace.Services.Identity.Infrastructure.Mongo.Queries.Handlers
{
    internal sealed class GetOnlineUsersHandler : IQueryHandler<GetOnlineUsers, PagedResult<UserDto>>
    {
        private readonly IMongoRepository<UserDocument, Guid> _userRepository;

        public GetOnlineUsersHandler(IMongoRepository<UserDocument, Guid> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PagedResult<UserDto>> HandleAsync(GetOnlineUsers query, CancellationToken cancellationToken)
        {
            var filter = Builders<UserDocument>.Filter.Eq(u => u.IsOnline, true);
            
            var totalResults = await _userRepository.Collection.CountDocumentsAsync(filter);
            
            var totalPages = (int)Math.Ceiling(totalResults / (double)query.PageSize);

            var onlineUsers = await _userRepository.Collection
                .Find(filter)
                .SortByDescending(u => u.LastActive)  
                .Skip((query.Page - 1) * query.PageSize)
                .Limit(query.PageSize)  
                .ToListAsync();  

            return PagedResult<UserDto>.Create(
                onlineUsers.Select(user => user.AsDto()),  
                query.Page,
                query.PageSize,
                totalPages,
                totalResults
            );
        }
    }
}
