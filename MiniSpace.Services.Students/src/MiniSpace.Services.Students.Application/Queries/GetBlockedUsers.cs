using Convey.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetBlockedUsers : IQuery<PagedResult<BlockedUserDto>>
    {
        public Guid BlockerId { get; set; }  
        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        public string OrderBy { get; set; }  
        public string SortOrder { get; set; }  

        public GetBlockedUsers(Guid blockerId, int page, int resultsPerPage, string orderBy = "BlockedAt", string sortOrder = "desc")
        {
            BlockerId = blockerId;
            Page = page;
            ResultsPerPage = resultsPerPage;
            OrderBy = orderBy;
            SortOrder = sortOrder;
        }
    }    
}
