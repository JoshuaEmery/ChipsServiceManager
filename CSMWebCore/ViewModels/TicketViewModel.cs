using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class TicketViewModel
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public string TicketStatus { get; set; }
        public bool NeedsBackup { get; set; }
        public string CheckedIn { get; set; }
        public string Finished { get; set; }
        public string CheckedOut { get; set; }
        public string CheckInUserId { get; set; }
        public string CheckOutUserId { get; set; }
    }
}
