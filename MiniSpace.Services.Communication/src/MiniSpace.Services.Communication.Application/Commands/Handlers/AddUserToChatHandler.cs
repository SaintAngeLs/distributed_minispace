using Convey.CQRS.Commands;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Core.Entities;
using MiniSpace.Services.Communication.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class AddUserToChatHandler : ICommandHandler<AddUserToChat>
    {
        private readonly IUserChatsRepository _userChatsRepository;

        public AddUserToChatHandler(IUserChatsRepository userChatsRepository)
        {
            _userChatsRepository = userChatsRepository;
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
        }
    }
}
