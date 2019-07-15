using CSMWebCore.Entities;
using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class LogEditViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TicketId { get; set; }
        public int TicketNumber { get; set; }
        public Ticket Ticket { get; set; }
        public DateTime Logged { get; set; }
        public string Notes { get; set; }
        public LogType LogType { get; set; }
        public ContactMethod ContactMethod { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public Customer Customer { get; set; }
    }
}
