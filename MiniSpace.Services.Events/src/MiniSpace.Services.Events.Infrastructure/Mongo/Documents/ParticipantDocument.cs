using System;
using System.Diagnostics.CodeAnalysis;
using Convey.Types;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
     public class ParticipantDocument
    {
        public Guid StudentId { get; set; }

        public static ParticipantDocument FromEntity(Participant participant)
        {
            return new ParticipantDocument
            {
                StudentId = participant.StudentId,
            };
        }

        public Participant ToEntity()
        {
            return new Participant(StudentId);
        }

    }

}