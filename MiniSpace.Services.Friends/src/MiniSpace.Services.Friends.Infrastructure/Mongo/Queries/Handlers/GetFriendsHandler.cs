using Paralax.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Core.Wrappers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Queries.Handlers
{
    public class GetFriendsHandler : IQueryHandler<GetFriends, PagedResponse<UserFriendsDto>>
    {
        private readonly IUserFriendsRepository _userFriendsRepository;
        private readonly ILogger<GetFriendsHandler> _logger;

        public GetFriendsHandler(IUserFriendsRepository userFriendsRepository, ILogger<GetFriendsHandler> logger)
        {
            _userFriendsRepository = userFriendsRepository;
            _logger = logger;
        }

        public async Task<PagedResponse<UserFriendsDto>> HandleAsync(GetFriends query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetFriends query for UserId: {UserId}", query.UserId);

            var allFriends = await _userFriendsRepository.GetFriendsAsync(query.UserId);

            if (allFriends == null || !allFriends.Any())
            {
                _logger.LogWarning("No friends found for UserId: {UserId}", query.UserId);
                return new PagedResponse<UserFriendsDto>(Enumerable.Empty<UserFriendsDto>(), query.Page, query.PageSize, 0);
            }

            var totalItems = allFriends.Count();

            var friendsToReturn = allFriends
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(f => new FriendDto
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    FriendId = f.FriendId,
                    CreatedAt = f.CreatedAt,
                    State = f.FriendState
                })
                .ToList();

            var userFriendsDto = new UserFriendsDto
            {
                UserId = query.UserId,
                Friends = friendsToReturn
            };

            var userFriendsDtos = new List<UserFriendsDto> { userFriendsDto };

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(userFriendsDtos, jsonOptions);
            _logger.LogInformation("Serialized UserFriendsDto JSON: {JsonString}", jsonString);

            return new PagedResponse<UserFriendsDto>(userFriendsDtos, query.Page, query.PageSize, totalItems);
        }
    }
}
