using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Organizations;

namespace MiniSpace.Web.Areas.Organizations.CommandsDto
{
    public class UpdateOrganizationSettingsCommand
    {
        public Guid OrganizationId { get; }
        public OrganizationSettingsDto Settings { get; }

        public UpdateOrganizationSettingsCommand(Guid organizationId, OrganizationSettingsDto settings)
        {
            OrganizationId = organizationId;
            Settings = settings;
        }
    }
}
