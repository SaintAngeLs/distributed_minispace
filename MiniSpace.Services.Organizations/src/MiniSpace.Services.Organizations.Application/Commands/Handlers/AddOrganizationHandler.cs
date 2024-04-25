using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class AddOrganizationHandler : ICommandHandler<AddOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IAppContext _appContext;

        public AddOrganizationHandler(IOrganizationRepository organizationRepository, IAppContext appContext)
        {
            _organizationRepository = organizationRepository;
            _appContext = appContext;
        }

        public async Task HandleAsync(AddOrganization command, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if(identity.IsAuthenticated && !identity.IsAdmin)
            {
                throw new Exceptions.UnauthorizedAccessException("admin");
            }
            
            var organization = new Organization(command.OrganizationId, command.Name, command.ParentId);
            if(command.ParentId != Guid.Empty)
            {
                var parent = await _organizationRepository.GetAsync(command.ParentId);
                if(parent is null)
                {
                    throw new ParentOrganizationNotFoundException(command.ParentId);
                }
            }
            await _organizationRepository.AddAsync(organization);
        }
    }
}