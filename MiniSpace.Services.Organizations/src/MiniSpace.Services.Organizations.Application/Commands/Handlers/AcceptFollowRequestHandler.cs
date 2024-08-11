using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class AcceptFollowRequestHandler : ICommandHandler<AcceptFollowRequest>
    {
        private readonly IOrganizationRequestsRepository _organizationRequestsRepository;
        private readonly IOrganizationMembersRepository _organizationMembersRepository;

        public AcceptFollowRequestHandler(
            IOrganizationRequestsRepository organizationRequestsRepository,
            IOrganizationMembersRepository organizationMembersRepository)
        {
            _organizationRequestsRepository = organizationRequestsRepository;
            _organizationMembersRepository = organizationMembersRepository;
        }

        public async Task HandleAsync(AcceptFollowRequest command, CancellationToken cancellationToken)
        {
            var request = await _organizationRequestsRepository.GetRequestAsync(command.OrganizationId, command.RequestId);
            if (request == null)
            {
                throw new OrganizationRequestNotFoundException(command.RequestId);
            }

            if (request.State != RequestState.Pending)
            {
                throw new InvalidRequestStateException(request.State, "Cannot accept a request that is not pending.");
            }

            // Approve the request
            request.Approve();
            await _organizationRequestsRepository.UpdateRequestAsync(command.OrganizationId, request);

            // Add the user as a member of the organization
            var newUser = new User(request.UserId, new Role("User", "Default role for organization members", new Dictionary<Permission, bool>()));
            await _organizationMembersRepository.AddMemberAsync(command.OrganizationId, newUser);
        }
    }
}
