using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.DTO.Organizations;

namespace Astravent.Web.Wasm.Areas.Organizations.CommandsDto
{
    public class SetOrganizationPrivacyCommand
    {
        public Guid OrganizationId { get; }
        public bool IsPrivate { get; }

        public SetOrganizationPrivacyCommand(Guid organizationId, bool isPrivate)
        {
            OrganizationId = organizationId;
            IsPrivate = isPrivate;
        }
    }
}
