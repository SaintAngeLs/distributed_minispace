using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class CreateChatHandler : ICommandHandler<CreateChat>
    {
        private readonly ICommunicationService _communicationService;
        private readonly ILogger<CreateChatHandler> _logger;

        public CreateChatHandler(ICommunicationService communicationService, ILogger<CreateChatHandler> logger)
        {
            _communicationService = communicationService;
            _logger = logger;
        }

        public async Task HandleAsync(CreateChat command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling CreateChat command for Chat ID: {command.ChatId}");

            // Call the CommunicationService to create the chat
            var chatId = await _communicationService.CreateChatAsync(command.ChatId, command.ParticipantIds, command.ChatName);

            _logger.LogInformation($"Chat created with ID: {chatId}");
        }
    }
}
