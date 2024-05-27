// using Convey.CQRS.Queries;
// using MiniSpace.Services.Email.Application.Dto;
// using MiniSpace.Services.Email.Core.Entities;
// using System;

// namespace MiniSpace.Services.Email.Application.Queries
// {
//     public class GetEmailNotificationsByUser : IQuery<PagedResult<EmailNotificationDto>>
//     {
//         public Guid UserId { get; set; }
//         public DateTime? StartDate { get; set; }
//         public DateTime? EndDate { get; set; }
//         public string Status { get; set; }
//         public int Page { get; set; } = 1;
//         public int ResultsPerPage { get; set; } = 10;
//         public string OrderBy { get; set; } = "CreatedAt"; 
//         public string SortOrder { get; set; } = "desc";    
//     }
// }
