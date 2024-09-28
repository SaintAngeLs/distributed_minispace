using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.DTO.Organizations;

namespace Astravent.Web.Wasm.Areas.Organizations.CommandsDto
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
