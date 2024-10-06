using System.Threading.Tasks;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Communication.Application.Dto;
using MiniSpace.Services.Communication.Application.Queries;
using MiniSpace.Services.Communication.Core.Repositories;
using MiniSpace.Services.Communication.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Communication.Infrastructure.Mongo.Queries.Handlers
{
    public class GetChatByIdHandler : IQueryHandler<GetChatById, ChatDto>
    {
        private readonly IUserChatsRepository _userChatsRepository;
        private readonly IOrganizationChatsRepository _organizationChatsRepository;

        public GetChatByIdHandler(IUserChatsRepository userChatsRepository, IOrganizationChatsRepository organizationChatsRepository)
        {
            _userChatsRepository = userChatsRepository;
            _organizationChatsRepository = organizationChatsRepository;
        }

        public async Task<ChatDto> HandleAsync(GetChatById query, CancellationToken cancellationToken)
        {
            var userChat = await _userChatsRepository.GetByUserIdAsync(query.ChatId);
            var chat = userChat?.GetChatById(query.ChatId) ?? (await _organizationChatsRepository.GetByOrganizationIdAsync(query.ChatId))?.GetChatById(query.ChatId);

            return chat?.AsDocument().AsEntity().AsDto();
        }
    }
}
