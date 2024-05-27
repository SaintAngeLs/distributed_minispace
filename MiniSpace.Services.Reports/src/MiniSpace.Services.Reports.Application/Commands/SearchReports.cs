using Convey.CQRS.Commands;
using MiniSpace.Services.Reports.Application.DTO;

namespace MiniSpace.Services.Reports.Application.Commands
{
    public class SearchReports : ICommand
    {
        public IEnumerable<string> ContextTypes { get; set; }
        public IEnumerable<string> States { get; set; }
        public Guid ReviewerId { get; set; }
        public PageableDto Pageable { get; set; }
    }
}