using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.DTO.Organizations;

namespace Astravent.Web.Wasm.Areas.Organizations.CommandsDto
{
    public class ManageFeedCommand
    {
        public Guid OrganizationId { get; }
        public string Content { get; }
        public string Action { get; }

        public ManageFeedCommand(Guid organizationId, string content, string action)
        {
            OrganizationId = organizationId;
            Content = content;
            Action = action;
        }
    }
}
