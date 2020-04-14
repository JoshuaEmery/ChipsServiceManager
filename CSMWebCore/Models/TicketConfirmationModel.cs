using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Models
{
    public class TicketConfirmationModel
    {
        public int ticketId { get; set; }
        public int deviceId { get; set; }
        public int customerId { get; set; }
        public Guid updateId { get; set; }        
    }
}
