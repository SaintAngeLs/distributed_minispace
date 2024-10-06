using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Identity.Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Threading;

namespace MiniSpace.Services.Identity.Application.Commands.Handlers
{
    internal sealed class VerifyEmailHandler : ICommandHandler<VerifyEmail>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<VerifyEmailHandler> _logger;

        public VerifyEmailHandler(IIdentityService identityService, ILogger<VerifyEmailHandler> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }

        public async Task HandleAsync(VerifyEmail command, CancellationToken cancellationToken)
        {
            await _identityService.VerifyEmailAsync(command);
            _logger.LogInformation($"Email verification for token: {command.Token}, email: {command.Email}, and hashed token: {command.HashedToken} processed.");
        }
    }
}
