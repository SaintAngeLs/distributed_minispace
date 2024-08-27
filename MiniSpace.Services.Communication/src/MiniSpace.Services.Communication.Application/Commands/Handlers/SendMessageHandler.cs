using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Events;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Core.Entities;
using MiniSpace.Services.Communication.Core.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class SendMessageHandler : ICommandHandler<SendMessage>
    {
        private readonly IUserChatsRepository _userChatsRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<SendMessageHandler> _logger;

        public SendMessageHandler(IUserChatsRepository userChatsRepository, IMessageBroker messageBroker, ILogger<SendMessageHandler> logger)
        {
            _userChatsRepository = userChatsRepository;
            _messageBroker = messageBroker;
            _logger = logger;
        }

        public async Task HandleAsync(SendMessage command, CancellationToken cancellationToken)
        {
            var senderChats = await _userChatsRepository.GetByUserIdAsync(command.SenderId);
            var chat = senderChats?.GetChatById(command.ChatId);

            if (chat == null)
            {
                _logger.LogWarning($"Chat with id {command.ChatId} not found for user with id {command.SenderId}");
                return;
            }

            var message = new Message(
                chatId: command.ChatId,
                senderId: command.SenderId,
                receiverId: command.SenderId, 
                content: command.Content,
                type: Enum.Parse<MessageType>(command.MessageType)
            );

            _logger.LogInformation($"Sending message with id {message.Id} to chat with id {command.ChatId}");


            chat.AddMessage(message);

            foreach (var participantId in chat.ParticipantIds)
            {
                var participantChats = await _userChatsRepository.GetByUserIdAsync(participantId);
                if (participantChats == null)
                {
                    participantChats = new UserChats(participantId);
                    _logger.LogInformation($"Creating new UserChats for user with id {participantId}");
                    participantChats.AddChat(new Chat(chat.ParticipantIds));
                }

                var participantChat = participantChats.GetChatById(command.ChatId);
                if (participantChat == null)
                {
                    participantChats.AddChat(new Chat(chat.ParticipantIds));
                    _logger.LogInformation($"Creating new Chat with id {command.ChatId} for user with id {participantId}");
                    participantChat = participantChats.GetChatById(command.ChatId);
                }

                participantChat.AddMessage(message);
                _logger.LogInformation($"Adding message with id {message.Id} to chat with id {command.ChatId} for user with id {participantId}");
                /* 
                    * This is a bug. The participantChats object is not being updated in the database.
                    * The UpdateAsync method should be called on the _userChatsRepository object. Here I am talking about the
                    * udpated verion of the UpdateAsync method which should not has the 2-nd varian of the implementation:
                    * - It should update the user Chats messages not the only user chats
                */
                await _userChatsRepository.UpdateAsync(participantChats);
            }

            // Publish the MessageSent event
            await _messageBroker.PublishAsync(new MessageSent(
                chatId: command.ChatId,
                messageId: message.Id,
                senderId: command.SenderId,
                content: command.Content
            ));
        }
    }
}
