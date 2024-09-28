using System;
using MiniSpace.Web.DTO.Organizations;

namespace MiniSpace.Web.Areas.Organizations.CommandsDto
{
    public class UpdateOrganizationCommand
    {
        public Guid OrganizationId { get; private set; }
        public string Name { get; }
        public string Description { get; }
        public Guid RootId { get; }
        public Guid ParentId { get; }
        public Guid OwnerId { get; }
        public OrganizationSettingsDto Settings { get; }
        public string BannerUrl { get; }
        public string ImageUrl { get; }
        public string DefaultRoleName { get; }

        public string Address { get; }
        public string Country { get; }
        public string City { get; }
        public string Telephone { get; }
        public string Email { get; }

        public UpdateOrganizationCommand(Guid organizationId, string name, string description,
            Guid rootId, Guid parentId, Guid ownerId, OrganizationSettingsDto settings,
            string bannerUrl, string imageUrl, string defaultRoleName, 
            string address, string country, string city, string telephone, string email)
        {
            OrganizationId = organizationId == Guid.Empty ? Guid.NewGuid() : organizationId;
            Name = name;
            Description = description;
            RootId = rootId;
            ParentId = parentId;
            OwnerId = ownerId;
            Settings = settings;
            BannerUrl = bannerUrl;
            ImageUrl = imageUrl;
            DefaultRoleName = defaultRoleName;

            Address = address;
            Country = country;
            City = city;
            Telephone = telephone;
            Email = email;
        }
    }
}
