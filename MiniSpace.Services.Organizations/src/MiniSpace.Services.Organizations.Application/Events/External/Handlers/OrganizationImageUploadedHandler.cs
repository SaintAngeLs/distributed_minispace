using Convey.CQRS.Events;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Application.Events.External.Handlers
{
    public class OrganizationImageUploadedHandler : IEventHandler<OrganizationImageUploaded>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationGalleryRepository _organizationGalleryRepository;

        public OrganizationImageUploadedHandler(
            IOrganizationRepository organizationRepository,
            IOrganizationGalleryRepository organizationGalleryRepository)
        {
            _organizationRepository = organizationRepository;
            _organizationGalleryRepository = organizationGalleryRepository;
        }

        public async Task HandleAsync(OrganizationImageUploaded @event, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetAsync(@event.OrganizationId);
            if (organization == null)
            {
                // Handle the case where the organization is not found
                return;
            }

            if (@event.ImageType == "OrganizationProfileImage")
            {
                organization.SetProfileImage(@event.ImageUrl);
                await _organizationRepository.UpdateAsync(organization);
            }
            else if (@event.ImageType == "OrganizationBannerImage")
            {
                organization.SetBannerImage(@event.ImageUrl);
                await _organizationRepository.UpdateAsync(organization);
            }
            else if (@event.ImageType == "OrganizationGalleryImage")
            {
                var galleryImage = new GalleryImage(new Guid(), @event.ImageUrl, @event.UploadDate);
                await _organizationGalleryRepository.AddImageAsync(@event.OrganizationId, galleryImage);
            }
        }
    }
}
