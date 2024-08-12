using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Enums;

namespace MiniSpace.Web.Areas.Events.CommandsDto
{
    public class UpdateEventCommand
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public OrganizerType OrganizerType { get; set; }
        public Guid OrganizerId { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid RootOrganizationId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string BuildingName { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public IEnumerable<string> MediaFilesUrl { get; set; }
        public string BannerUrl { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public decimal Fee { get; set; }
        public string Category { get; set; }
        public string PublishDate { get; set; }
        public EventVisibility Visibility { get; set; }
        public EventSettings Settings { get; set; }
    }
}
