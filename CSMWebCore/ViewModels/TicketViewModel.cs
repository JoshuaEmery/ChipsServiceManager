using CSMWebCore.Entities;
using CSMWebCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class TicketViewModel
    {
        public Ticket Ticket { get; set; }
        public Customer Customer { get; set; }
        public Log Log { get; set; }
        public IEnumerable<Log> ServiceLogs { get; set; }
        public IEnumerable<Log> ContactLogs { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public DateFilter DateFilter { get; set; }
        public string Status { get; set; }
        public bool All { get; set; }
        public TimeSpan DaysIdle { get; set; }
    }
}
