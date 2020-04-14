using CSMWebCore.Entities;
using CSMWebCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class TicketEditViewModel
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public bool NeedsBackup { get; set; }
        public DateTime CheckedIn { get; set; }
        public DateTime Finished { get; set; }
        public DateTime CheckedOut { get; set; }
        public string CheckInUserId { get; set; }
        public string CheckOutUserId { get; set; }
    }
}
