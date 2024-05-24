using Convey.CQRS.Commands;
using MiniSpace.Services.Reports.Application.DTO;

namespace MiniSpace.Services.Reports.Application.Commands
{
    public class SearchReports : ICommand
    {
        public PageableDto Pageable { get; set; }
    }
}