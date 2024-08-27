using Convey.CQRS.Commands;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Events;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Core.Entities;
using MiniSpace.Services.Communication.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class SendMessageHandler : ICommandHandler<SendMessage>
    {
        private readonly IUserChatsRepository _userChatsRepository;
        private readonly IMessageBroker _messageBroker;

        public SendMessageHandler(IUserChatsRepository userChatsRepository, IMessageBroker messageBroker)
        {
            _userChatsRepository = userChatsRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(SendMessage command, CancellationToken cancellationToken)
        {
            var userChats = await _userChatsRepository.GetByUserIdAsync(command.SenderId);
            var chat = userChats?.GetChatById(command.ChatId);

            if (chat != null)
            {
                var message = new Message(command.ChatId, command.SenderId, command.SenderId, command.Content, Enum.Parse<MessageType>(command.MessageType));
                chat.AddMessage(message);
                await _userChatsRepository.UpdateAsync(userChats);

                await _messageBroker.PublishAsync(new MessageSent(command.ChatId, message.Id, command.SenderId, command.Content));
            }
        }
    }
}
