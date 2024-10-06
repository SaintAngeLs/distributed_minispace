using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.DTO.Organizations;

namespace Astravent.Web.Wasm.Areas.Organizations.CommandsDto
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
