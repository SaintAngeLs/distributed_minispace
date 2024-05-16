using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Application.Events;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Application.Services;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class CreateOrganizationHandler : ICommandHandler<CreateOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public CreateOrganizationHandler(IOrganizationRepository organizationRepository, IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _organizationRepository = organizationRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreateOrganization command, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if(identity.IsAuthenticated && !identity.IsAdmin)
            {
                throw new Exceptions.UnauthorizedAccessException("admin");
            }
            
            var root = await _organizationRepository.GetAsync(command.RootId);
            if(root is null)
            {
                throw new RootOrganizationNotFoundException(command.RootId);
            }

            var parent = root.GetSubOrganization(command.ParentId);
            if(parent is null)
            {
                throw new ParentOrganizationNotFoundException(command.ParentId);
            }
            
            var organization = new Organization(command.OrganizationId, command.Name);  
            parent.AddSubOrganization(organization);
            await _organizationRepository.UpdateAsync(root);
            await _messageBroker.PublishAsync(new OrganizationCreated(organization.Id, organization.Name, parent.Id));
        }
    }
}