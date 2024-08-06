using System;
using System.Collections;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class UpdateEvent : ICommand
    {
        public Guid EventId { get; }
        public string Name { get; }
        public OrganizerType OrganizerType { get; } 
        public Guid OrganizerId { get; }
        public Guid OrganizationId { get; }
        public Guid RootOrganizationId { get; }
        public string StartDate { get; }
        public string EndDate { get; }
        public string BuildingName { get; }
        public string Street { get; }
        public string BuildingNumber { get; }
        public string ApartmentNumber { get; }
        public string City { get; }
        public string ZipCode { get; }
        public string Country { get; }  
        public IEnumerable<string> MediaFilesUrl { get; }
        public string BannerUrl { get; } 
        public string Description { get; }
        public int Capacity { get; }
        public decimal Fee { get; }
        public string Category { get; }
        public string PublishDate { get; }
        public Visibility Visibility { get; }
        public EventSettings Settings { get; }

        public UpdateEvent(Guid eventId, string name, OrganizerType organizerType, Guid organizerId, Guid organizationId, 
            Guid rootOrganizationId, string startDate, string endDate, string buildingName, string street, 
            string buildingNumber, string apartmentNumber, string city, string zipCode, string country, IEnumerable<string> mediaFiles, 
            string bannerUrl, string description, int capacity, decimal fee, string category, string publishDate, 
            Visibility visibility, EventSettings settings)
        {
            EventId = eventId;
            Name = name;
            OrganizerType = organizerType; 
            OrganizerId = organizerId;
            OrganizationId = organizationId;
            RootOrganizationId = rootOrganizationId;
            StartDate = startDate;
            EndDate = endDate;
            BuildingName = buildingName;
            Street = street;
            BuildingNumber = buildingNumber;
            ApartmentNumber = apartmentNumber;
            City = city;
            ZipCode = zipCode;
            Country = country; 
            MediaFilesUrl = mediaFiles;
            BannerUrl = bannerUrl; 
            Description = description;
            Capacity = capacity;
            Fee = fee;
            Category = category;
            PublishDate = publishDate;
            Visibility = visibility; 
            Settings = settings;
        }
    }
}
