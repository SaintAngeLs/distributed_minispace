using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Queries
{
    public class GetStudentEvents : IQuery<IEnumerable<EventDto>>
    {
        public Guid StudentId { get; set; }
    }
}