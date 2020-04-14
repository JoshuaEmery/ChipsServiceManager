using CSMWebCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Entities
{
    public class Log : IHasId
    {
        public int Id { get; set; }
        public Ticket Ticket { get; set; }
        public int TicketId { get; set; }
        public Event Event { get; set; }
        public int EventId { get; set; }

        public TicketStatus TicketStatus { get; set; }
        // Each log added to a ticket stores the next status and the latest log keeps the current ticket status.
        // This way, Log becomes a kind of history table for Ticket.

        // This property is never directly modified by the user through a dropdown. In some cases, the status
        // can be automatically intuited by the controller action used. In other cases if multiple statuses are
        // possible, only the possible statuses are selectable in the view (an event for malware removal has
        // either "In Progress" or "Needs Contact".

        [Display(Name = "Technician")]
        public string UserCreated { get; set; }
        public DateTime DateCreated { get; set; }

        // future properties if we allow editing of a log note
        //public string UserModified { get; set; }
        //public DateTime DateModified { get; set; }

        //[Required(ErrorMessage = "Notes are required on all log entries")]
        // notes are only required on some types of events
        public string Notes { get; set; }

        // redundant propeties to remove after switch to Log/Event
        public LogType LogType { get; set; }
        public ContactMethod ContactMethod { get; set; }
    }
}
