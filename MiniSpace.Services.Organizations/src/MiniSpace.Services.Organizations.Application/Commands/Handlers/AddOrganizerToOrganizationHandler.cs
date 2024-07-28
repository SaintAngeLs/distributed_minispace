using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Application.Events;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Application.Services;
using MiniSpace.Services.Organizations.Core.Repositories;
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class AddOrganizerToOrganizationHandler : ICommandHandler<AddOrganizerToOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizerRepository _organizerRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        
        public AddOrganizerToOrganizationHandler(IOrganizationRepository organizationRepository, 
            IOrganizerRepository organizerRepository, IAppContext appContext, IMessageBroker messageBroker)
        {
            _organizationRepository = organizationRepository;
            _organizerRepository = organizerRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(AddOrganizerToOrganization command, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated)
            {
                throw new Exceptions.UnauthorizedAccessException("user");
            }
            
            var root = await _organizationRepository.GetAsync(command.RootOrganizationId);
            if (root is null)
            {
                throw new RootOrganizationNotFoundException(command.RootOrganizationId);
            }

            var organization = root.GetSubOrganization(command.OrganizationId);
            if (organization == null)
            {
                throw new OrganizationNotFoundException(command.OrganizationId);
            }
    
            var organizer = await _organizerRepository.GetAsync(command.OrganizerId);
            if (organizer is null)
            {
                throw new OrganizerNotFoundException(command.OrganizerId);
            }

            organization.AddOrganizer(command.OrganizerId);
            await _organizationRepository.UpdateAsync(root);
            await _messageBroker.PublishAsync(new OrganizerAddedToOrganization(organization.Id, organizer.Id));
        }
    }
}