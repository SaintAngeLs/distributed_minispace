using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class RejectFollowRequestHandler : ICommandHandler<RejectFollowRequest>
    {
        private readonly IOrganizationRequestsRepository _organizationRequestsRepository;

        public RejectFollowRequestHandler(IOrganizationRequestsRepository organizationRequestsRepository)
        {
            _organizationRequestsRepository = organizationRequestsRepository;
        }

        public async Task HandleAsync(RejectFollowRequest command, CancellationToken cancellationToken)
        {
            var request = await _organizationRequestsRepository.GetRequestAsync(command.OrganizationId, command.RequestId);
            if (request == null)
            {
                throw new OrganizationRequestNotFoundException(command.RequestId);
            }

            if (request.State != RequestState.Pending)
            {
                throw new InvalidRequestStateException(request.State, "Cannot reject a request that is not pending.");
            }

            // Reject the request
            request.Reject(command.Reason);
            await _organizationRequestsRepository.UpdateRequestAsync(command.OrganizationId, request);
        }
    }
}
