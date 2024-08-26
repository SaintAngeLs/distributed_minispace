using Convey.CQRS.Commands;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class UpdateMessageStatusHandler : ICommandHandler<UpdateMessageStatus>
    {
        private readonly IUserChatsRepository _userChatsRepository;

        public UpdateMessageStatusHandler(IUserChatsRepository userChatsRepository)
        {
            _userChatsRepository = userChatsRepository;
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
                }
            }
        }
    }
}
