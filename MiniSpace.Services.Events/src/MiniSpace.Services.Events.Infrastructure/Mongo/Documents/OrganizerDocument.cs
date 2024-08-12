using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Convey.Types;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class OrganizerDocument
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? OrganizationId { get; set; }
        public OrganizerType OrganizerType { get; set; }

        public static OrganizerDocument FromEntity(Organizer organizer)
        {
            return new OrganizerDocument
            {
                Id = organizer.Id,
                UserId = organizer.UserId,
                OrganizationId = organizer.OrganizationId,
                OrganizerType = organizer.OrganizerType
            };
        }

        public Organizer ToEntity()
        {
            return new Organizer(Id, OrganizerType, UserId, OrganizationId);
        }
    }
}