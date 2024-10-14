using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Communication.Application.Dto;
using MiniSpace.Services.Communication.Application.Queries;
using MiniSpace.Services.Communication.Core.Repositories;
using MiniSpace.Services.Communication.Infrastructure.Mongo.Documents;


namespace MiniSpace.Services.Communication.Infrastructure.Mongo.Queries.Handlers
{
    public class GetMessagesForChatHandler : IQueryHandler<GetMessagesForChat, IEnumerable<MessageDto>>
    {
        private readonly IUserChatsRepository _userChatsRepository;

        public GetMessagesForChatHandler(IUserChatsRepository userChatsRepository)
        {
            _userChatsRepository = userChatsRepository;
        }

        public async Task<IEnumerable<MessageDto>> HandleAsync(GetMessagesForChat query, CancellationToken cancellationToken)
        {
            var chat = await _userChatsRepository.GetByChatIdAsync(query.ChatId);

            if (chat != null)
            {
                var messages = chat.Messages.Select(m => m.AsDto()).ToList();
                return messages;
            }

            return Enumerable.Empty<MessageDto>();
        }
    }
}
