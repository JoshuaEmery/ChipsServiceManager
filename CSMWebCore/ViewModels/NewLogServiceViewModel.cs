using CSMWebCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class NewLogServiceViewModel
    {
        public int TicketNumber { get; set; }
        public int TicketId { get; set; }
        public string LogNotes { get; set; }
        public EventName EventName { get; set; }
        public LogType LogType { get; set; }
        public TicketStatus TicketStatus { get; set; }

    }
}
