using System.Collections;
using System.Collections.Generic;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class EventSettings
    {

        public bool RequiresApproval { get; set; }
        public bool IsOnlineEvent { get; set; }

        public bool IsPrivate { get; set; } // Indicates if the event is private
        public bool RequiresRSVP { get; set; } // Whether RSVP is required to attend
        public bool AllowsGuests { get; set; } // Whether attendees can bring guests
        public bool ShowAttendeesPublicly { get; set; } // If true, the list of attendees is visible to others

        public bool SendReminders { get; set; } // Whether to send reminders to participants
        public int ReminderDaysBefore { get; set; } // Number of days before the event to send reminders
        public bool EnableChat { get; set; } // Whether to enable a chat for the event
        public bool AllowComments { get; set; } // Whether participants can leave comments

        public bool RequiresPayment { get; set; } // If true, payment is required to attend
        public bool IssueTickets { get; set; } // Whether tickets are issued for the event
        public int MaxTicketsPerPerson { get; set; } // Maximum number of tickets one person can purchase
        public decimal TicketPrice { get; set; } // Price per ticket if payment is required

        public bool RecordEvent { get; set; } // Whether to record the event

        // Custom settings
        public string CustomTermsAndConditions { get; set; } // Custom terms and conditions for the event
        public IDictionary<string, string> CustomFields { get; set; } 

        // Constructor with default values
        public EventSettings()
        {
            // Default settings
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
            IssueTickets = true;
            MaxTicketsPerPerson = 5;
            TicketPrice = 0m;
            RecordEvent = false;
            CustomTermsAndConditions = string.Empty;
            CustomFields = new Dictionary<string, string>();
        }
    }
}
