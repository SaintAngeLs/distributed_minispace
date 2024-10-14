using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Identity.Application.Services;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace MiniSpace.Services.Identity.Application.Commands.Handlers
{
    internal sealed class GenerateTwoFactorSecretHandler : ICommandHandler<GenerateTwoFactorSecret>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<GenerateTwoFactorSecretHandler> _logger;

        public GenerateTwoFactorSecretHandler(IIdentityService identityService, ILogger<GenerateTwoFactorSecretHandler> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }

        public async Task HandleAsync(GenerateTwoFactorSecret command, CancellationToken cancellationToken = default)   
        {
            var secret = await _identityService.GenerateTwoFactorSecretAsync(command);
            _logger.LogInformation($"Generated a new two-factor authentication secret for user ID: {command.UserId}");
        }
    }
}
