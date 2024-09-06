using Convey.CQRS.Commands;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Events;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class DeleteChatHandler : ICommandHandler<DeleteChat>
    {
        private readonly IUserChatsRepository _userChatsRepository;
        private readonly IMessageBroker _messageBroker;

        public DeleteChatHandler(IUserChatsRepository userChatsRepository, IMessageBroker messageBroker)
        {
            _userChatsRepository = userChatsRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeleteChat command, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Received DeleteChat command - ChatId: {command.ChatId}, UserId: {command.UserId}");

            await _userChatsRepository.DeleteChatAsync(command.UserId, command.ChatId);

            await _messageBroker.PublishAsync(new ChatDeleted(command.ChatId));
        }
    }
}
