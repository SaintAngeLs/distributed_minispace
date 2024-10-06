using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Identity.Application.Services;
using System.Threading;

[assembly: InternalsVisibleTo("MiniSpace.Services.Identity.Application.UnitTests")]
namespace MiniSpace.Services.Identity.Application.Commands.Handlers
{
    // Simple wrapper
    internal sealed class SignUpHandler : ICommandHandler<SignUp>
    {
        private readonly IIdentityService _identityService;

        public SignUpHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public Task HandleAsync(SignUp command, CancellationToken cancellationToken) => _identityService.SignUpAsync(command);
    }
}