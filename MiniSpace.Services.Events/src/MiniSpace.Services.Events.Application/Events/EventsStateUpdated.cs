﻿using System;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventsStateUpdated(DateTime updateDateTime) : IEvent
    {
        public DateTime UpdateDateTime { get; set; } = updateDateTime;
    }
}