using System;
using System.Collections;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class CreateEvent : ICommand
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public string OrganizerType { get; set; }  
        public Guid OrganizerId { get; set; }
        public Guid? OrganizationId { get; set; }
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
        public string Visibility { get; set; } 
        public EventSettingsCommand Settings { get; set; }

        public class EventSettingsCommand
        {
            public bool RequiresApproval { get; set; }
            public bool IsOnlineEvent { get; set; }
            public bool IsPrivate { get; set; }
            public bool RequiresRSVP { get; set; }
            public bool AllowsGuests { get; set; }
            public bool ShowAttendeesPublicly { get; set; }
            public bool SendReminders { get; set; }
            public int ReminderDaysBefore { get; set; }
            public bool EnableChat { get; set; }
            public bool AllowComments { get; set; }
            public bool RequiresPayment { get; set; }
            public string PaymentMethod { get; set; }
            public string PaymentReceiverDetails { get; set; }
            public string PaymentGateway { get; set; }
            public bool IssueTickets { get; set; }
            public int MaxTicketsPerPerson { get; set; }
            public decimal TicketPrice { get; set; }
            public bool RecordEvent { get; set; }
            public string CustomTermsAndConditions { get; set; }
            public IDictionary<string, string> CustomFields { get; set; } = new Dictionary<string, string>();
        }

    }
}
