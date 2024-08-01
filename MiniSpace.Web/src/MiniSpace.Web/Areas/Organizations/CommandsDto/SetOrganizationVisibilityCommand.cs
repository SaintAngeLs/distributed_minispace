using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Organizations;

namespace MiniSpace.Web.Areas.Organizations.CommandsDto
{
    public class SetOrganizationVisibilityCommand
    {
        public Guid OrganizationId { get; }
        public bool IsVisible { get; }

        public SetOrganizationVisibilityCommand(Guid organizationId, bool isVisible)
        {
            OrganizationId = organizationId;
            IsVisible = isVisible;
        }
    }
}
