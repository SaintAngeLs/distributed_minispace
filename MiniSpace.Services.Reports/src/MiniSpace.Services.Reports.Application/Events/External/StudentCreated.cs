﻿using System;
using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Reports.Application.Events.External
{
    [Message("students")]
    public class StudentCreated : IEvent
    {
        public Guid StudentId { get; }
        public string Name { get; }

        public StudentCreated(Guid studentId, string name)
        {
            StudentId = studentId;
            Name = name;
        }
    }
}