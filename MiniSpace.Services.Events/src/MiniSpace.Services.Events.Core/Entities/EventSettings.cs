using System.Collections;
using System.Collections.Generic;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class EventSettings
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
        public PaymentMethod PaymentMethod { get; set; }
        public string PaymentReceiverDetails { get; set; } 
        public string PaymentGateway { get; set; } 
        public bool IssueTickets { get; set; }
        public int MaxTicketsPerPerson { get; set; } 
        public decimal TicketPrice { get; set; } 

        public bool RecordEvent { get; set; }

        public string CustomTermsAndConditions { get; set; }
        public IDictionary<string, string> CustomFields { get; set; } 

        public EventSettings()
        {
            RequiresApproval = false;
            IsOnlineEvent = false;
            IsPrivate = false;
            RequiresRSVP = false;
            AllowsGuests = true;
            ShowAttendeesPublicly = true;
            SendReminders = true;
            ReminderDaysBefore = 1;
            EnableChat = true;
            AllowComments = true;
            RequiresPayment = false;
            PaymentMethod = PaymentMethod.Online;
            PaymentReceiverDetails = string.Empty;
            PaymentGateway = "Stripe";
            IssueTickets = true;
            MaxTicketsPerPerson = 5;
            TicketPrice = 0m;
            RecordEvent = false;
            CustomTermsAndConditions = string.Empty;
            CustomFields = new Dictionary<string, string>();
        }
    }
}
