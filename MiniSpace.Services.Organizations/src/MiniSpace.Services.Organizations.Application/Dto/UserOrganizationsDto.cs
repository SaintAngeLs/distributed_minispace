using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class UserOrganizationsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid OwnerId { get; set; }
        public string BannerUrl { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<UserOrganizationsDto> SubOrganizations { get; set; }

        public UserOrganizationsDto()
        {
        }

        public UserOrganizationsDto(Organization organization)
        {
            Id = organization.Id;
            Name = organization.Name;
            Description = organization.Description;
            OwnerId = organization.OwnerId;
            BannerUrl = organization.BannerUrl;
            ImageUrl = organization.ImageUrl;
            SubOrganizations = organization.SubOrganizations?.Select(o => new UserOrganizationsDto(o)).ToList();
        }
    }
}
