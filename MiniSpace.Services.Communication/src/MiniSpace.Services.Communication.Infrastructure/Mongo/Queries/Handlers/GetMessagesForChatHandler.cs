using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MiniSpace.Services.Communication.Application.Dto;
using MiniSpace.Services.Communication.Application.Queries;
using MiniSpace.Services.Communication.Core.Repositories;
using MiniSpace.Services.Communication.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Communication.Infrastructure.Mongo.Queries.Handlers
{
    public class GetMessagesForChatHandler : IQueryHandler<GetMessagesForChat, IEnumerable<MessageDto>>
    {
        private readonly IUserChatsRepository _userChatsRepository;
        private readonly IOrganizationChatsRepository _organizationChatsRepository;

        public GetMessagesForChatHandler(IUserChatsRepository userChatsRepository, IOrganizationChatsRepository organizationChatsRepository)
        {
            _userChatsRepository = userChatsRepository;
            _organizationChatsRepository = organizationChatsRepository;
        }

        public async Task<IEnumerable<MessageDto>> HandleAsync(GetMessagesForChat query, CancellationToken cancellationToken)
        {
            var userChat = await _userChatsRepository.GetByUserIdAsync(query.ChatId);
            var chat = userChat?.GetChatById(query.ChatId) ?? (await _organizationChatsRepository.GetByOrganizationIdAsync(query.ChatId))?.GetChatById(query.ChatId);

            return chat?.Messages.Select(m => m.AsDocument().AsEntity().AsDto()) ?? Enumerable.Empty<MessageDto>();
        }
    }
}
