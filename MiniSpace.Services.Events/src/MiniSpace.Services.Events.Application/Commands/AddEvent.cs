using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class AddEvent : ICommand
    {
        public Guid EventId { get; }
        public string Name { get; }
        public string OrganizerId { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public string Description { get; }
        public int Capacity { get; }
        public decimal Fee { get; }
        public string Category { get; }
    }
}