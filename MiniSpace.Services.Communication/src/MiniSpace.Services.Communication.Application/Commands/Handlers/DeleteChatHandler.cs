using Convey.CQRS.Commands;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class DeleteChatHandler : ICommandHandler<DeleteChat>
    {
        private readonly IUserChatsRepository _userChatsRepository;

        public DeleteChatHandler(IUserChatsRepository userChatsRepository)
        {
            _userChatsRepository = userChatsRepository;
        }

        public async Task HandleAsync(DeleteChat command, CancellationToken cancellationToken)
        {
            await _userChatsRepository.DeleteChatAsync(command.ChatId, command.ChatId);
        }
    }
}
