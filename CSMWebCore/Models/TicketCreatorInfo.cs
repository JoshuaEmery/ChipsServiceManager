using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Models
{
    public class TicketCreatorInfo
    {
        public int DeviceId { get; set; }
        public int CustomerId { get; set; }
        public bool NeedsBackup { get; set; }
        public string Notes { get; set; }
        public string UserName { get; set; }

    }
}
