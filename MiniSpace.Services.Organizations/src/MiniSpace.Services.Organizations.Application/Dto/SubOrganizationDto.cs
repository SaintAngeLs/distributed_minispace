using System;
using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class SubOrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BannerUrl { get; set; }
        public string ImageUrl { get; set; }
        public Guid OwnerId { get; set; }

        public SubOrganizationDto()
        {
        }

        public SubOrganizationDto(Organization organization)
        {
            Id = organization.Id;
            Name = organization.Name;
            Description = organization.Description;
            BannerUrl = organization.BannerUrl;
            ImageUrl = organization.ImageUrl;
            OwnerId = organization.OwnerId;
        }
    }
}
