using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class SearchEvents : ICommand
    {
        public string Name { get; set; }
        public string Organizer { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public PageableDto Pageable { get; set; }
    }
}