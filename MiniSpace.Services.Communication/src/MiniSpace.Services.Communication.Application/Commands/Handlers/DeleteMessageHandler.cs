using Paralax.CQRS.Commands;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Core.Repositories;
using MiniSpace.Services.Communication.Application.Events;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class DeleteMessageHandler : ICommandHandler<DeleteMessage>
    {
        private readonly IUserChatsRepository _userChatsRepository;
        private readonly IMessageBroker _messageBroker;

        public DeleteMessageHandler(IUserChatsRepository userChatsRepository, IMessageBroker messageBroker)
        {
            _userChatsRepository = userChatsRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeleteMessage command, CancellationToken cancellationToken)
        {
            var userChats = await _userChatsRepository.GetByUserIdAsync(command.ChatId);
            var chat = userChats?.GetChatById(command.ChatId);

            if (chat != null)
            {
                var message = chat.Messages.Find(m => m.Id == command.MessageId);
                if (message != null)
                {
                    chat.Messages.Remove(message);
                    await _userChatsRepository.UpdateAsync(userChats);

                    await _messageBroker.PublishAsync(new MessageStatusUpdated(command.ChatId, command.MessageId, "Deleted"));
                }
            }
        }
    }
}
