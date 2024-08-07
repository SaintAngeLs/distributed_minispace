using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.DTO
{
     public class EventSettingsDto
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
        public IDictionary<string, string> CustomFields { get; set; }

        public EventSettingsDto(EventSettings settings)
        {
            RequiresApproval = settings.RequiresApproval;
            IsOnlineEvent = settings.IsOnlineEvent;
            IsPrivate = settings.IsPrivate;
            RequiresRSVP = settings.RequiresRSVP;
            AllowsGuests = settings.AllowsGuests;
            ShowAttendeesPublicly = settings.ShowAttendeesPublicly;
            SendReminders = settings.SendReminders;
            ReminderDaysBefore = settings.ReminderDaysBefore;
            EnableChat = settings.EnableChat;
            AllowComments = settings.AllowComments;
            RequiresPayment = settings.RequiresPayment;
            PaymentMethod = settings.PaymentMethod.ToString();;
            PaymentReceiverDetails = settings.PaymentReceiverDetails;
            PaymentGateway = settings.PaymentGateway;
            IssueTickets = settings.IssueTickets;
            MaxTicketsPerPerson = settings.MaxTicketsPerPerson;
            TicketPrice = settings.TicketPrice;
            RecordEvent = settings.RecordEvent;
            CustomTermsAndConditions = settings.CustomTermsAndConditions;
            CustomFields = settings.CustomFields ?? new Dictionary<string, string>();
        }
    }
}