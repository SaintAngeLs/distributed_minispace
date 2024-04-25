using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class AddOrganizationHandler : ICommandHandler<AddOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public AddOrganizationHandler(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task HandleAsync(AddOrganization command)
        {
            var organization = new Organization(command.OrganizationId, command.Name, command.ParentId);
            await _organizationRepository.AddAsync(organization);
        }
    }
}