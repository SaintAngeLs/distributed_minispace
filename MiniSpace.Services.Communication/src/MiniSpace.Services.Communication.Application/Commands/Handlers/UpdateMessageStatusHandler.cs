using Paralax.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Events;
using MiniSpace.Services.Communication.Application.Hubs;
using MiniSpace.Services.Communication.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Communication.Application.Commands.Handlers
{
    public class UpdateMessageStatusHandler : ICommandHandler<UpdateMessageStatus>
    {
        private readonly ICommunicationService _communicationService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<UpdateMessageStatusHandler> _logger;

        public UpdateMessageStatusHandler(
            ICommunicationService communicationService,
            IHubContext<ChatHub> hubContext,
            ILogger<UpdateMessageStatusHandler> logger)
        {
            _communicationService = communicationService;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateMessageStatus command, CancellationToken cancellationToken)
        {
            await _communicationService.UpdateMessageStatusAsync(command.ChatId, command.MessageId, command.Status);
            _logger.LogInformation($"Message status updated: ChatId={command.ChatId}, MessageId={command.MessageId}, Status={command.Status}");
            await ChatHub.SendMessageStatusUpdate(_hubContext, command.ChatId.ToString(), command.MessageId, command.Status, _logger);
        }
    }
}
