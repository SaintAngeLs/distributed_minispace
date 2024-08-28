using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
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

        public GetMessagesForChatHandler(IUserChatsRepository userChatsRepository)
        {
            _userChatsRepository = userChatsRepository;
        }

        public async Task<IEnumerable<MessageDto>> HandleAsync(GetMessagesForChat query, CancellationToken cancellationToken)
        {
            // Retrieve the Chat object by ChatId
            var chat = await _userChatsRepository.GetByChatIdAsync(query.ChatId);

            if (chat != null)
            {
                // Convert messages to DTOs
                var messages = chat.Messages.Select(m => m.AsDto()).ToList();

                // Serialize the response to JSON and log it
                var jsonResponse = JsonSerializer.Serialize(messages, new JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine("Messages JSON:");
                Console.WriteLine(jsonResponse);

                return messages;
            }

            // If the chat was not found, return an empty list
            return Enumerable.Empty<MessageDto>();
        }
    }
}
