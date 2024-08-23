using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Application.Events;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class AcceptFollowRequestHandler : ICommandHandler<AcceptFollowRequest>
    {
        private readonly IOrganizationRequestsRepository _organizationRequestsRepository;
        private readonly IOrganizationMembersRepository _organizationMembersRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationRolesRepository _organizationRolesRepository;
        private readonly IUserOrganizationsRepository _userOrganizationsRepository;
        private readonly IEventDispatcher _eventDispatcher;

        public AcceptFollowRequestHandler(
            IOrganizationRequestsRepository organizationRequestsRepository,
            IOrganizationMembersRepository organizationMembersRepository,
            IOrganizationRepository organizationRepository,
            IOrganizationRolesRepository organizationRolesRepository,
            IUserOrganizationsRepository userOrganizationsRepository,
            IEventDispatcher eventDispatcher)
        {
            _organizationRequestsRepository = organizationRequestsRepository;
            _organizationMembersRepository = organizationMembersRepository;
            _organizationRepository = organizationRepository;
            _organizationRolesRepository = organizationRolesRepository;
            _userOrganizationsRepository = userOrganizationsRepository;
            _eventDispatcher = eventDispatcher;
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

            // Retrieve the organization
            var organization = await _organizationRepository.GetAsync(command.OrganizationId);
            if (organization == null)
            {
                throw new OrganizationNotFoundException(command.OrganizationId);
            }

            // Retrieve the default role from the organization
            var defaultRole = await _organizationRolesRepository.GetRoleByNameAsync(command.OrganizationId, organization.DefaultRoleName);
            if (defaultRole == null)
            {
                throw new RoleNotFoundException(organization.DefaultRoleName);
            }

            // Add the user as a member of the organization with the default role
            var newUser = new User(request.UserId, defaultRole);
            await _organizationMembersRepository.AddMemberAsync(command.OrganizationId, newUser);

            // Add the organization to the user's list of organizations
            await _userOrganizationsRepository.AddOrganizationToUserAsync(request.UserId, command.OrganizationId);

            // Publish event
            await _eventDispatcher.PublishAsync(new UserAddedToOrganization(command.OrganizationId, request.UserId, DateTime.UtcNow));
        }
    }

}
