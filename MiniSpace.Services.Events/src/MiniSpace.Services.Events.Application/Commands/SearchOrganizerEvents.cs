using System;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class SearchOrganizerEvents : ICommand
    {
        public string Name { get; set; }
        public Guid OrganizerId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string State { get; set; }
        public PageableDto Pageable { get; set; }
    }
}