﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Wrappers;

namespace MiniSpace.Services.Events.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetStudentEvents : IQuery<PagedResponse<IEnumerable<EventDto>>>
    {
        public Guid StudentId { get; set; }
        public string EngagementType { get; set; }
        public int Page { get; set; }
        public int NumberOfResults { get; set; }
    }
}