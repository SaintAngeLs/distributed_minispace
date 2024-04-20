using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Wrappers;

namespace MiniSpace.Services.Events.Application.Queries
{
    public class GetStudentEvents : IQuery<PagedResponse<IEnumerable<EventDto>>>
    {
        public Guid StudentId { get; set; }
        public int numberOfResults { get; set; }
    }
}