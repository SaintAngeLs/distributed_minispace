using Convey.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;
using System.Collections.Generic;

namespace MiniSpace.Services.Friends.Application.Queries
{
    public class GetFriends : IQuery<IEnumerable<StudentFriendsDto>>
    {
        public Guid StudentId { get; set; }
    }    
}