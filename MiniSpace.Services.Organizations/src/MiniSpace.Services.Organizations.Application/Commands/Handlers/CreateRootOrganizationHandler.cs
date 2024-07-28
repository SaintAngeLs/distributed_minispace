using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Application.Events;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Application.Services;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class CreateRootOrganizationHandler : ICommandHandler<CreateRootOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public CreateRootOrganizationHandler(IOrganizationRepository organizationRepository, IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _organizationRepository = organizationRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreateRootOrganization command, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if(!identity.IsAuthenticated)
            {
                throw new Exceptions.UnauthorizedAccessException("user");
            }

            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new InvalidOrganizationNameException(command.Name);
            }
            
            var organization = new Organization(command.OrganizationId, command.Name);
            await _organizationRepository.AddAsync(organization);
            await _messageBroker.PublishAsync(new RootOrganizationCreated(organization.Id, organization.Name));
        }
    }
}