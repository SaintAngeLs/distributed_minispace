﻿using Convey.CQRS.Queries;
using MiniSpace.Services.Reports.Application.DTO;
using MiniSpace.Services.Reports.Core.Wrappers;

namespace MiniSpace.Services.Reports.Application.Queries
{
    public class GetStudentReports : IQuery<PagedResponse<ReportDto>>
    {
        public Guid UserId { get; set; }
        public int Page { get; set; }
        public int Results { get; set; }
    }
}