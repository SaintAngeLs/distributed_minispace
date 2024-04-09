using System;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class AddEvent : ICommand
    {
        public Guid EventId { get; }
        public string Name { get; }
        public string OrganizerId { get; }
        public string StartDate { get; }
        public string EndDate { get; }
        public string BuildingName { get; }
        public string Street { get; }
        public string BuildingNumber { get; }
        public string ApartmentNumber { get; }
        public string City { get; }
        public string ZipCode { get; }
        public string Description { get; }
        public int Capacity { get; }
        public decimal Fee { get; }
        public string Category { get; }
    }
}