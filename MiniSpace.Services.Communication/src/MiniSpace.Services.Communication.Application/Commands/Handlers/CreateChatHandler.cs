using Convey.CQRS.Commands;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Events;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Core.Entities;
using MiniSpace.Services.Communication.Core.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class CreateChatHandler : ICommandHandler<CreateChat>
    {
        private readonly IUserChatsRepository _userChatsRepository;
        private readonly IMessageBroker _messageBroker;

        public CreateChatHandler(IUserChatsRepository userChatsRepository, IMessageBroker messageBroker)
        {
            _userChatsRepository = userChatsRepository;
            _messageBroker = messageBroker;
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

            await _messageBroker.PublishAsync(new ChatCreated(command.ChatId, command.ParticipantIds));
        }
    }
}
