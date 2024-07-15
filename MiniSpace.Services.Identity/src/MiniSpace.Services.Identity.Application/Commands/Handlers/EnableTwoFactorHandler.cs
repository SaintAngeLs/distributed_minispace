using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Identity.Application.Services;
using Microsoft.Extensions.Logging;

namespace MiniSpace.Services.Identity.Application.Commands.Handlers
{
    internal sealed class EnableTwoFactorHandler : ICommandHandler<EnableTwoFactor>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<EnableTwoFactorHandler> _logger;

        public EnableTwoFactorHandler(IIdentityService identityService, ILogger<EnableTwoFactorHandler> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }

        public async Task HandleAsync(EnableTwoFactor command)
        {
            await _identityService.EnableTwoFactorAsync(command);
            _logger.LogInformation($"Two-factor authentication enabled for user ID: {command.UserId}");
        }
    }
}
