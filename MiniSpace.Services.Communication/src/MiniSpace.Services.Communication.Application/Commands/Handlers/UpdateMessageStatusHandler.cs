using Convey.CQRS.Commands;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Events;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class UpdateMessageStatusHandler : ICommandHandler<UpdateMessageStatus>
    {
        private readonly IUserChatsRepository _userChatsRepository;
        private readonly IMessageBroker _messageBroker;

        public UpdateMessageStatusHandler(IUserChatsRepository userChatsRepository, IMessageBroker messageBroker)
        {
            _userChatsRepository = userChatsRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdateMessageStatus command, CancellationToken cancellationToken)
        {
            var userChats = await _userChatsRepository.GetByUserIdAsync(command.ChatId);
            var chat = userChats?.GetChatById(command.ChatId);

            if (chat != null)
            {
                var message = chat.Messages.Find(m => m.Id == command.MessageId);
                if (message != null)
                {
                    switch (command.Status)
                    {
                        case "Read":
                            message.MarkAsRead();
                            break;
                        case "Unread":
                            message.MarkAsUnread();
                            break;
                        case "Deleted":
                            message.MarkAsDeleted();
                            break;
                    }

                    await _userChatsRepository.UpdateAsync(userChats);

                    await _messageBroker.PublishAsync(new MessageStatusUpdated(command.ChatId, command.MessageId, command.Status));
                }
            }
        }
    }
}
