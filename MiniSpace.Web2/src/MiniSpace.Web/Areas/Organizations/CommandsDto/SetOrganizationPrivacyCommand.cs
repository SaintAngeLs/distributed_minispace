using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Organizations;

namespace MiniSpace.Web.Areas.Organizations.CommandsDto
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
