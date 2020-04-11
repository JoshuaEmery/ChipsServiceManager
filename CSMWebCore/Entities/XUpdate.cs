using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Entities
{
    public class XUpdate
    {
        public Guid Id { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
