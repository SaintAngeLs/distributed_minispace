using Paralax.CQRS.Commands;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Events;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class RemoveUserFromChatHandler : ICommandHandler<RemoveUserFromChat>
    {
        private readonly IUserChatsRepository _userChatsRepository;
        private readonly IMessageBroker _messageBroker;

        public RemoveUserFromChatHandler(IUserChatsRepository userChatsRepository, IMessageBroker messageBroker)
        {
            _userChatsRepository = userChatsRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(RemoveUserFromChat command, CancellationToken cancellationToken)
        {
            var userChats = await _userChatsRepository.GetByUserIdAsync(command.UserId);
            var chat = userChats?.GetChatById(command.ChatId);

            if (chat != null)
            {
                chat.ParticipantIds.Remove(command.UserId);
                await _userChatsRepository.UpdateAsync(userChats);

                await _messageBroker.PublishAsync(new UserAddedToChat(command.ChatId, command.UserId));
            }
        }
    }
}
