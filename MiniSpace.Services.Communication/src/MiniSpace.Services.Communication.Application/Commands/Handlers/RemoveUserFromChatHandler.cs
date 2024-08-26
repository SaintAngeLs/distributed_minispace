using Convey.CQRS.Commands;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class RemoveUserFromChatHandler : ICommandHandler<RemoveUserFromChat>
    {
        private readonly IUserChatsRepository _userChatsRepository;

        public RemoveUserFromChatHandler(IUserChatsRepository userChatsRepository)
        {
            _userChatsRepository = userChatsRepository;
        }

        public async Task HandleAsync(RemoveUserFromChat command, CancellationToken cancellationToken)
        {
            var userChats = await _userChatsRepository.GetByUserIdAsync(command.UserId);
            var chat = userChats?.GetChatById(command.ChatId);

            if (chat != null)
            {
                chat.ParticipantIds.Remove(command.UserId);
                await _userChatsRepository.UpdateAsync(userChats);
            }
        }
    }
}
