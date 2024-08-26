using Convey.CQRS.Commands;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Core.Entities;
using MiniSpace.Services.Communication.Core.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class CreateChatHandler : ICommandHandler<CreateChat>
    {
        private readonly IUserChatsRepository _userChatsRepository;

        public CreateChatHandler(IUserChatsRepository userChatsRepository)
        {
            _userChatsRepository = userChatsRepository;
        }

        public async Task HandleAsync(CreateChat command, CancellationToken cancellationToken)
        {
            var chat = new Chat(command.ParticipantIds);
            
            foreach (var participantId in command.ParticipantIds)
            {
                var userChats = await _userChatsRepository.GetByUserIdAsync(participantId) ?? new UserChats(participantId);
                userChats.AddChat(chat);
                await _userChatsRepository.AddOrUpdateAsync(userChats);
            }
        }
    }
}
