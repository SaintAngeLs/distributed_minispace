using Paralax.CQRS.Events;
using MiniSpace.Services.Organizations.Core.Repositories;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Application.Events.External.Handlers
{
    public class MediaFileDeletedHandler : IEventHandler<MediaFileDeleted>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationGalleryRepository _organizationGalleryRepository;

        public MediaFileDeletedHandler(
            IOrganizationRepository organizationRepository,
            IOrganizationGalleryRepository organizationGalleryRepository)
        {
            _organizationRepository = organizationRepository;
            _organizationGalleryRepository = organizationGalleryRepository;
        }

        public async Task HandleAsync(MediaFileDeleted @event, CancellationToken cancellationToken)
        {
            var eventJson = JsonSerializer.Serialize(@event, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine("Received MediaFileDeleted event: " + eventJson);

            if (@event.OrganizationId == null)
            {
                return; // If there's no OrganizationId, this event is not relevant to the organization service.
            }

            var organization = await _organizationRepository.GetAsync(@event.OrganizationId.Value);
            if (organization == null)
            {
                return; // Organization not found
            }

            // Determine the type of media file that was deleted and update the organization accordingly.
            if (@event.Source == "OrganizationProfileImage")
            {
                if (organization.ImageUrl == @event.MediaFileUrl)
                {
                    organization.SetProfileImage(null);
                    await _organizationRepository.UpdateAsync(organization);
                }
            }
            else if (@event.Source == "OrganizationBannerImage")
            {
                if (organization.BannerUrl == @event.MediaFileUrl)
                {
                    organization.SetBannerImage(null);
                    await _organizationRepository.UpdateAsync(organization);
                }
            }
            else if (@event.Source == "OrganizationGalleryImage")
            {
                // Remove the image from the organization's gallery if it exists
                await _organizationGalleryRepository.RemoveImageAsync(@event.OrganizationId.Value, @event.MediaFileUrl);
            }
        }
    }
}
