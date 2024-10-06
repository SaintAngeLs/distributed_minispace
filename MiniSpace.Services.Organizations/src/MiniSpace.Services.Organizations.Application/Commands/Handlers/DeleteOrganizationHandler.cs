using Paralax.CQRS.Commands;
using MiniSpace.Services.Organizations.Application.Events;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Application.Services;
using MiniSpace.Services.Organizations.Core.Repositories;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class DeleteOrganizationHandler:ICommandHandler<DeleteOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        
        public DeleteOrganizationHandler(IOrganizationRepository organizationRepository, IAppContext appContext, IMessageBroker messageBroker)
        {
            _organizationRepository = organizationRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(DeleteOrganization command, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if(!identity.IsAuthenticated)
            {
                throw new UserUnauthorizedAccessException("user");
            }
            
            var root = await _organizationRepository.GetAsync(command.RootId);
            if(root is null)
            {
                throw new RootOrganizationNotFoundException(command.RootId);
            }

            var organization = root.GetSubOrganization(command.OrganizationId);
            if(organization is null)
            {
                throw new OrganizationNotFoundException(command.OrganizationId);
            }

            if (root.Id.Equals(organization.Id))
            {
                await _organizationRepository.DeleteAsync(root.Id);
            }
            else
            {
                root.RemoveChildOrganization(organization);
                await _organizationRepository.UpdateAsync(root);
            }
            
            await _messageBroker.PublishAsync(new OrganizationDeleted(organization.Id));
        }
    }
}