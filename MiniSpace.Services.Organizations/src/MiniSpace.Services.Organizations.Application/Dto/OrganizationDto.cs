using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MiniSpace.Services.Organizations.Application.DTO
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

        // New fields
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public IEnumerable<UserDto> Users { get; set; }

        public OrganizationDto()
        {
        }

        public OrganizationDto(Organization organization)
        {
            Id = organization.Id;
            Name = organization.Name;
            Description = organization.Description;
            BannerUrl = organization.BannerUrl;
            ImageUrl = organization.ImageUrl;
            OwnerId = organization.OwnerId;
            DefaultRoleName = organization.DefaultRoleName;

            // Initialize new fields
            Address = organization.Address;
            Country = organization.Country;
            City = organization.City;
            Telephone = organization.Telephone;
            Email = organization.Email;

            // Convert each User entity to a UserDto
            Users = organization.Users?.Select(user => new UserDto(user)).ToList();
        }
    }
}
