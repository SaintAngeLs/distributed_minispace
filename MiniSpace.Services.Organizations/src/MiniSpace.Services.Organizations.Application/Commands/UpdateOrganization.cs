using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class UpdateOrganization : ICommand
    {
        public Guid OrganizationId { get; private set; }
        public string Name { get; }
        public string Description { get; }
        public Guid RootId { get; }
        public Guid ParentId { get; }
        public Guid OwnerId { get; }
        public OrganizationSettings Settings { get; }
        public string BannerUrl { get; }
        public string ImageUrl { get; }
        public string DefaultRoleName { get; }

        // New fields
        public string Address { get; }
        public string Country { get; }
        public string City { get; }
        public string Telephone { get; }
        public string Email { get; }

        public UpdateOrganization(Guid organizationId, string name, string description,
            Guid rootId, Guid parentId, Guid ownerId, OrganizationSettings settings,
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

            // New fields
            Address = address;
            Country = country;
            City = city;
            Telephone = telephone;
            Email = email;
        }
    }
}
