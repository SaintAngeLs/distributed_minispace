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
        private readonly IOrganizationRolesRepository _organizationRolesRepository;

        public FollowOrganizationHandler(
            IOrganizationRepository organizationRepository,
            IOrganizationRequestsRepository organizationRequestsRepository,
            IOrganizationMembersRepository organizationMembersRepository,
            IOrganizationRolesRepository organizationRolesRepository)
        {
            _organizationRepository = organizationRepository;
            _organizationRequestsRepository = organizationRequestsRepository;
            _organizationMembersRepository = organizationMembersRepository;
            _organizationRolesRepository = organizationRolesRepository;
        }

        public async Task HandleAsync(FollowOrganization command, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetAsync(command.OrganizationId);
            if (organization == null)
            {
                throw new OrganizationNotFoundException(command.OrganizationId);
            }

            var existingMember = await _organizationMembersRepository.GetMemberAsync(command.OrganizationId, command.UserId);
            if (existingMember != null)
            {
                throw new UserAlreadyMemberException(command.UserId, command.OrganizationId);
            }

            if (organization.Settings.IsPrivate)
            {
                var existingRequests = await _organizationRequestsRepository.GetRequestsAsync(command.OrganizationId);
                var userRequest = existingRequests.FirstOrDefault(r => r.UserId == command.UserId && r.State == RequestState.Pending);
                
                if (userRequest != null)
                {
                    throw new UserAlreadyRequestedException(command.UserId, command.OrganizationId);
                }

                var request = OrganizationRequest.CreateNew(command.UserId, "Request to follow organization");
                await _organizationRequestsRepository.AddRequestAsync(command.OrganizationId, request);
            }
            else
            {
                // Retrieve the default role from the organization
                var defaultRole = await _organizationRolesRepository.GetRoleByNameAsync(command.OrganizationId, organization.DefaultRoleName);
                
                if (defaultRole == null)
                {
                    throw new RoleNotFoundException(organization.DefaultRoleName);
                }

                var newUser = new User(command.UserId, defaultRole);
                await _organizationMembersRepository.AddMemberAsync(command.OrganizationId, newUser);
            }
        }
    }
}
