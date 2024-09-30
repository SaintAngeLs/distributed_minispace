using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Core.Wrappers;

namespace MiniSpace.Services.Students.Application.Queries.Handlers
{
    public class GetBlockedUsersHandler : IQueryHandler<GetBlockedUsers, PagedResponse<BlockedUserDto>>
    {
        private readonly IBlockedUsersRepository _blockedUsersRepository;

        public GetBlockedUsersHandler(IBlockedUsersRepository blockedUsersRepository)
        {
            _blockedUsersRepository = blockedUsersRepository;
        }

        public async Task<PagedResponse<BlockedUserDto>> HandleAsync(GetBlockedUsers query, CancellationToken cancellationToken = default)
        {
            var blockedUsersAggregate = await _blockedUsersRepository.GetAsync(query.BlockerId);

            if (blockedUsersAggregate == null || !blockedUsersAggregate.BlockedUsersList.Any())
            {
                return new PagedResponse<BlockedUserDto>(
                    Enumerable.Empty<BlockedUserDto>(), 
                    query.Page, 
                    query.ResultsPerPage, 
                    0);
            }

            var sortedBlockedUsers = query.SortOrder.ToLower() == "asc" 
                ? blockedUsersAggregate.BlockedUsersList.OrderBy(b => b.BlockedAt)
                : blockedUsersAggregate.BlockedUsersList.OrderByDescending(b => b.BlockedAt);

            var totalItems = sortedBlockedUsers.Count();
            var totalPages = (int)System.Math.Ceiling(totalItems / (double)query.ResultsPerPage);

            var blockedUsersPage = sortedBlockedUsers
                .Skip((query.Page - 1) * query.ResultsPerPage)
                .Take(query.ResultsPerPage)
                .Select(b => new BlockedUserDto
                {
                    BlockerId = query.BlockerId,
                    BlockedUserId = b.BlockedUserId,
                    BlockedAt = b.BlockedAt
                })
                .ToList();

            return new PagedResponse<BlockedUserDto>(
                blockedUsersPage,
                query.Page,
                query.ResultsPerPage,
                totalItems);
        }
    }
}
