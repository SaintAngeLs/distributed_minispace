using Convey.CQRS.Queries;
using System.Text.Json.Serialization; 
using MiniSpace.Services.Friends.Application.Dto;
using System.Collections.Generic;

namespace MiniSpace.Services.Friends.Application.Queries
{
    public class GetFriendRequests : IQuery<IEnumerable<FriendRequestDto>>, IQuery
    {
        public Guid UserId { get; set; }

        [JsonConstructor]
        public GetFriendRequests([property: JsonPropertyName("userId")] Guid userId)
        {
            UserId = userId;
        }
    }
}
