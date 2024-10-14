using System.Linq;
using System.Threading.Tasks;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Communication.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Communication.Application.Dto;
using MiniSpace.Services.Communication.Application.Queries;
using MiniSpace.Services.Communication.Core.Repositories;
using MiniSpace.Services.Communication.Core.Wrappers;

namespace MiniSpace.Services.Communication.Infrastructure.Mongo.Queries.Handlers
{
    public class GetUserChatsHandler : IQueryHandler<GetUserChats, PagedResponse<UserChatDto>>
    {
        private readonly IUserChatsRepository _userChatsRepository;

        public GetUserChatsHandler(IUserChatsRepository userChatsRepository)
        {
            _userChatsRepository = userChatsRepository;
        }

        public async Task<PagedResponse<UserChatDto>> HandleAsync(GetUserChats query, CancellationToken cancellationToken)
        {
            var userChats = await _userChatsRepository.GetByUserIdAsync(query.UserId);

            if (userChats == null || !userChats.Chats.Any())
            {
                return new PagedResponse<UserChatDto>(Enumerable.Empty<UserChatDto>(), 0, query.PageSize, 0);
            }

            var paginatedChats = userChats.Chats
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(chat => chat.AsDto()) 
                .ToList();

            var userChatDto = new UserChatDto
            {
                UserId = query.UserId,
                Chats = paginatedChats
            };

            return new PagedResponse<UserChatDto>(new List<UserChatDto> { userChatDto }, query.Page, query.PageSize, userChats.Chats.Count);
        }
    }
}
