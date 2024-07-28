using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Application.Events;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Application.Services;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class AssignRoleToMemberHandler : ICommandHandler<AssignRoleToMember>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationRolesRepository _organizationRolesRepository;
        private readonly IOrganizationMembersRepository _organizationMembersRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public AssignRoleToMemberHandler(
            IOrganizationRepository organizationRepository,
            IOrganizationRolesRepository organizationRolesRepository,
            IOrganizationMembersRepository organizationMembersRepository,
            IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _organizationRepository = organizationRepository;
            _organizationRolesRepository = organizationRolesRepository;
            _organizationMembersRepository = organizationMembersRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(AssignRoleToMember command, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if (!identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var organization = await _organizationRepository.GetAsync(command.OrganizationId);
            if (organization == null)
            {
                throw new OrganizationNotFoundException(command.OrganizationId);
            }

            var member = await _organizationMembersRepository.GetMemberAsync(command.OrganizationId, command.MemberId);
            if (member == null)
            {
                throw new MemberNotFoundException(command.MemberId);
            }

            var role = new Role(command.MemberId, command.Role);
            await _organizationRolesRepository.AddRoleAsync(role);

            organization.AssignRole(command.MemberId, command.Role);
            await _organizationRepository.UpdateAsync(organization);

            await _messageBroker.PublishAsync(new RoleAssignedToMember(organization.Id, command.MemberId, command.Role, DateTime.UtcNow));
        }
    }
}
