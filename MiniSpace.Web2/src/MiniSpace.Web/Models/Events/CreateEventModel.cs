using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Organizations;

namespace MiniSpace.Web.Models.Events
{
    public class CreateEventModel
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public Guid OrganizerId { get; set; }
        public OrganizationDto Organization { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BuildingName { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public IEnumerable<Guid> MediaFiles { get; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public decimal Fee { get; set; }
        public string Category { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
