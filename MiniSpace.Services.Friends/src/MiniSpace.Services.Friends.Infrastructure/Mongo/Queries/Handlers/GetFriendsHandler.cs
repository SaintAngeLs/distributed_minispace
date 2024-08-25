using Convey.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Queries.Handlers
{
    public class GetFriendsHandler : IQueryHandler<GetFriends, IEnumerable<UserFriendsDto>>
    {
        private readonly IUserFriendsRepository _studentFriendsRepository;

        public GetFriendsHandler(IUserFriendsRepository studentFriendsRepository)
        {
            _studentFriendsRepository = studentFriendsRepository;
        }

        public async Task<IEnumerable<UserFriendsDto>> HandleAsync(GetFriends query, CancellationToken cancellationToken)
        {
            var friends = await _studentFriendsRepository.GetFriendsAsync(query.UserId);
            if (!friends.Any())
            {
                return Enumerable.Empty<UserFriendsDto>();
            }

            return new List<UserFriendsDto>
            {
                new UserFriendsDto
                {
                    UserId = query.UserId,
                    Friends = friends.Select(f => new FriendDto
                    {
                        Id = f.Id,
                        UserId = f.UserId,
                        FriendId = f.FriendId,
                        CreatedAt = f.CreatedAt,
                        State = f.FriendState
                    }).ToList()
                }
            };
        }
    }

}
