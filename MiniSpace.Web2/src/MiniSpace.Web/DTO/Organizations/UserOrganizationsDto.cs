using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Web.DTO.Organizations
{
    public class UserOrganizationsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BannerUrl { get; set; }
        public string ImageUrl { get; set; }
        public Guid OwnerId { get; set; }
        public IEnumerable<UserOrganizationsDto> SubOrganizations { get; set; }
        public bool HasSubOrganizations => SubOrganizations != null && SubOrganizations.Any();

        public IEnumerable<OrganizationUserDto> Users { get; set; } = new List<OrganizationUserDto>();
        public int UserCount => Users?.Count() ?? 0;

        public IEnumerable<Guid> AllChildrenIds { get; set; } = new List<Guid>();
        public bool IsExpanded { get; set; } = false;
    }
}
