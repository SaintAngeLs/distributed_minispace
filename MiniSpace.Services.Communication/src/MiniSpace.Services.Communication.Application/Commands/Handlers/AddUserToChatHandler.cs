using Paralax.CQRS.Commands;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Events;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Core.Entities;
using MiniSpace.Services.Communication.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class AddUserToChatHandler : ICommandHandler<AddUserToChat>
    {
        private readonly IUserChatsRepository _userChatsRepository;
        private readonly IMessageBroker _messageBroker;

        public AddUserToChatHandler(IUserChatsRepository userChatsRepository, IMessageBroker messageBroker)
        {
            _userChatsRepository = userChatsRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(AddUserToChat command, CancellationToken cancellationToken)
        {
            var userChats = await _userChatsRepository.GetByUserIdAsync(command.UserId) ?? new UserChats(command.UserId);
            var chat = userChats.GetChatById(command.ChatId);

            if (chat == null)
            {
                chat = new Chat(new List<Guid> { command.UserId });
                userChats.AddChat(chat);
                await _userChatsRepository.AddOrUpdateAsync(userChats);
            }

            await _messageBroker.PublishAsync(new UserAddedToChat(command.ChatId, command.UserId));
        }
    }
}
