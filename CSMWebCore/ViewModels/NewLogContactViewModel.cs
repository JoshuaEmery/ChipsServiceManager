using CSMWebCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class NewLogContactViewModel
    {
        public int TicketNumber { get; set; }
        public int TicketId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string LogNotes { get; set; }
        public EventName EventName { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public ContactMethod ContactMethod { get; set; }

    }
}