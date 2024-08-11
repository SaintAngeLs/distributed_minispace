using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class FollowOrganizationHandler : ICommandHandler<FollowOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationRequestsRepository _organizationRequestsRepository;
        private readonly IOrganizationMembersRepository _organizationMembersRepository;

        public FollowOrganizationHandler(
            IOrganizationRepository organizationRepository,
            IOrganizationRequestsRepository organizationRequestsRepository,
            IOrganizationMembersRepository organizationMembersRepository)
        {
            _organizationRepository = organizationRepository;
            _organizationRequestsRepository = organizationRequestsRepository;
            _organizationMembersRepository = organizationMembersRepository;
        }

        public async Task HandleAsync(FollowOrganization command, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetAsync(command.OrganizationId);
            if (organization == null)
            {
                throw new OrganizationNotFoundException(command.OrganizationId);
            }

            // Check if the user is already a member
            var existingMember = await _organizationMembersRepository.GetMemberAsync(command.OrganizationId, command.UserId);
            if (existingMember != null)
            {
                throw new UserAlreadyMemberException(command.UserId, command.OrganizationId);
            }

            if (organization.Settings.IsPrivate)
            {
                // If the organization is private, check if the user has already requested access
                var existingRequests = await _organizationRequestsRepository.GetRequestsAsync(command.OrganizationId);
                var userRequest = existingRequests.FirstOrDefault(r => r.UserId == command.UserId && r.State == RequestState.Pending);
                
                if (userRequest != null)
                {
                    throw new UserAlreadyRequestedException(command.UserId, command.OrganizationId);
                }

                // Create a new request to join the organization
                var request = OrganizationRequest.CreateNew(command.UserId, "Request to follow organization");
                await _organizationRequestsRepository.AddRequestAsync(command.OrganizationId, request);
            }
            else
            {
                // If the organization is public, add the user as a member directly
                var newUser = new User(command.UserId, new Role("User", "Default role for organization members", new Dictionary<Permission, bool>()));
                await _organizationMembersRepository.AddMemberAsync(command.OrganizationId, newUser);
            }
        }
    }
}
