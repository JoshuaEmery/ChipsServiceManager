using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class TicketHomeViewModel
    {
        public Ticket Ticket { get; set; }
        public Customer Customer { get; set; }
        public Log Log { get; set; }
        public IEnumerable<Log> Logs { get; set; }
    }
}
