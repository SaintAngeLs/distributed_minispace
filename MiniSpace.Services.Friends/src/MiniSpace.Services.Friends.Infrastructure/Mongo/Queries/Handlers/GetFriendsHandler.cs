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
    public class GetFriendsHandler : IQueryHandler<GetFriends, IEnumerable<StudentFriendsDto>>
    {
        private readonly IStudentFriendsRepository _studentFriendsRepository;

        public GetFriendsHandler(IStudentFriendsRepository studentFriendsRepository)
        {
            _studentFriendsRepository = studentFriendsRepository;
        }

        public async Task<IEnumerable<StudentFriendsDto>> HandleAsync(GetFriends query, CancellationToken cancellationToken)
        {
            var friends = await _studentFriendsRepository.GetFriendsAsync(query.StudentId);
            if (!friends.Any())
            {
                return Enumerable.Empty<StudentFriendsDto>();
            }

            return new List<StudentFriendsDto>
            {
                new StudentFriendsDto
                {
                    StudentId = query.StudentId,
                    Friends = friends.Select(f => new FriendDto
                    {
                        Id = f.Id,
                        StudentId = f.StudentId,
                        FriendId = f.FriendId,
                        CreatedAt = f.CreatedAt,
                        State = f.FriendState
                    }).ToList()
                }
            };
        }
    }

}
