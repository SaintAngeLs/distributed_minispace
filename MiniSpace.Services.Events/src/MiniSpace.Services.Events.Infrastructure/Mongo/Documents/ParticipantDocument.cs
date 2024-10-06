using System;
using System.Diagnostics.CodeAnalysis;
using Paralax.Types;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
     public class ParticipantDocument
    {
        public Guid UserId { get; set; }

        public static ParticipantDocument FromEntity(Participant participant)
        {
            return new ParticipantDocument
            {
                UserId = participant.UserId,
            };
        }

        public Participant ToEntity()
        {
            return new Participant(UserId);
        }

    }

}