using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MiniSpace.Web.DTO.Enums;

namespace MiniSpace.Web.Areas.Events.CommandsDto
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
        
        public string PaymentMethod { get; set; } // This is a string
        
        public string PaymentReceiverDetails { get; set; } 
        public string PaymentGateway { get; set; }
        public bool IssueTickets { get; set; }
        public int MaxTicketsPerPerson { get; set; } 
        public decimal TicketPrice { get; set; } 

        public bool RecordEvent { get; set; }

        public string CustomTermsAndConditions { get; set; }
        public IDictionary<string, string> CustomFields { get; set; } 
    }

}
