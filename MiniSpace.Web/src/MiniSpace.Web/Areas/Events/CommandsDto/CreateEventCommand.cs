using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MiniSpace.Web.DTO.Enums;
using MiniSpace.Web.DTO.Events;

namespace MiniSpace.Web.Areas.Events.CommandsDto
{
    public class CreateEventCommand
    {
        public Guid EventId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string OrganizerType { get; set; }
        public Guid OrganizerId { get; set; }
        public Guid? OrganizationId { get; set; } 
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string BuildingName { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string BuildingNumber { get; set; } = string.Empty;
        public string ApartmentNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public IEnumerable<string> MediaFilesUrl { get; set; } = new List<string>();
        public string BannerUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal Fee { get; set; }
        public string Category { get; set; }
        public string PublishDate { get; set; } = string.Empty;
        public string Visibility { get; set; }
        
        public EventSettingsDto Settings { get; set; } = new EventSettingsDto(); 
    }

}
