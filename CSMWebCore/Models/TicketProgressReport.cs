using CSMWebCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Models
{
    public class TicketProgressReport
    {
        public int TicketId { get; set; }
        public Dictionary<TicketStatus, TimeSpan> TicketProgress = new Dictionary<TicketStatus, TimeSpan>();
    }
}
