using System;
using System.Collections;
using System.Collections.Generic;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class UpdateEvent : ICommand
    {
        public Guid EventId { get; }
        public string Name { get; }
        public Guid OrganizerId { get; }
        public string StartDate { get; }
        public string EndDate { get; }
        public string BuildingName { get; }
        public string Street { get; }
        public string BuildingNumber { get; }
        public string ApartmentNumber { get; }
        public string City { get; }
        public string ZipCode { get; }
        public IEnumerable<Guid> MediaFiles { get; }
        public string Description { get; }
        public int Capacity { get; }
        public decimal Fee { get; }
        public string Category { get; }
        public string PublishDate { get; }

        public UpdateEvent(Guid eventId, string name, Guid organizerId, string startDate, string endDate, 
            string buildingName, string street, string buildingNumber, string apartmentNumber, string city, 
            string zipCode, IEnumerable<Guid> mediaFiles, string description, int capacity, decimal fee, 
            string category, string publishDate)
        {
            EventId = eventId;
            Name = name;
            OrganizerId = organizerId;
            StartDate = startDate;
            EndDate = endDate;
            BuildingName = buildingName;
            Street = street;
            BuildingNumber = buildingNumber;
            ApartmentNumber = apartmentNumber;
            City = city;
            ZipCode = zipCode;
            MediaFiles = mediaFiles;
            Description = description;
            Capacity = capacity;
            Fee = fee;
            Category = category;
            PublishDate = publishDate;
        }
    }
}