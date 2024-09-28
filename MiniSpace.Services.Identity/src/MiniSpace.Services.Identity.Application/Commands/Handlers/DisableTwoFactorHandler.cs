using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Identity.Application.Services;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace MiniSpace.Services.Identity.Application.Commands.Handlers
{
    internal sealed class DisableTwoFactorHandler : ICommandHandler<DisableTwoFactor>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<DisableTwoFactorHandler> _logger;

        public DisableTwoFactorHandler(IIdentityService identityService, ILogger<DisableTwoFactorHandler> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }

        public async Task HandleAsync(DisableTwoFactor command, CancellationToken cancellationToken)
        {
            await _identityService.DisableTwoFactorAsync(command);
            _logger.LogInformation($"Two-factor authentication disabled for user ID: {command.UserId}");
        }
    }
}
