﻿using CSMWebCore.Models;
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
        public int TicketNumber { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public TicketStatus TicketStatus { get; set; }
        [Display(Name = "Data Backup Needed?")]
        public bool NeedsBackup { get; set; }
        public DateTime CheckedIn { get; set; }
        public DateTime Finished { get; set; }
        public DateTime CheckedOut { get; set; }
        public string CheckInUserId { get; set; }
        public string CheckOutUserId { get; set; }
    }
}
