using Convey.CQRS.Events;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Core.Repositories;

namespace MiniSpace.Services.Organizations.Application.Events.External.Handlers
{
    public class OrganizerRightsRevokedHandler : IEventHandler<OrganizerRightsRevoked>
    {
        private readonly IOrganizerRepository _organizerRepository;
        private readonly IOrganizationRepository _organizationRepository;
        
        public OrganizerRightsRevokedHandler(IOrganizerRepository organizerRepository, IOrganizationRepository organizationRepository)
        {
            _organizerRepository = organizerRepository;
            _organizationRepository = organizationRepository;
        }
        
        public async Task HandleAsync(OrganizerRightsRevoked @event, CancellationToken cancellationToken)
        {
            var organizer = await _organizerRepository.GetAsync(@event.UserId);
            if (organizer is null)
            {
                throw new OrganizerNotFoundException(@event.UserId);
            }
            
            var organizerOrganizations = await _organizationRepository.GetOrganizerOrganizationsAsync(@event.UserId);
            foreach (var organization in organizerOrganizations)
            {
                organization.RemoveOrganizer(organizer);
                await _organizationRepository.UpdateAsync(organization);
            }
            
            await _organizerRepository.DeleteAsync(@event.UserId);
        }
    }
}