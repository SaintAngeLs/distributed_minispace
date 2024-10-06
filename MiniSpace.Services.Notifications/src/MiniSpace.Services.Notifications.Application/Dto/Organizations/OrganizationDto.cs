using MiniSpace.Services.Notifications.Core.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MiniSpace.Services.Notifications.Application.Dto.Organizations
{
    [ExcludeFromCodeCoverage]
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BannerUrl { get; set; }
        public string ImageUrl { get; set; }
        public Guid OwnerId { get; set; }
        public string DefaultRoleName { get; set; }

        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public IEnumerable<OrganizationUserDto> Users { get; set; }
        public OrganizationSettingsDto Settings { get; set; }

        public OrganizationDto()
        {
        }
    }
}
