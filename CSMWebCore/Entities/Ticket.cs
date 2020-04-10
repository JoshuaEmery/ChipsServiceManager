using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        [Display(Name = "Ticket No.")]
        public int TicketNumber { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        [Display(Name = "Data Backup")]
        public bool NeedsBackup { get; set; }

        // properties which will be found in Log after switch to Log/Event
        [Display(Name = "Status")]
        public TicketStatus TicketStatus { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string CheckInUserId { get; set; }
        public string CheckOutUserId { get; set; }
    }
}
