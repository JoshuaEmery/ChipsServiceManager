using CSMWebCore.Entities;
using CSMWebCore.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class TicketViewModel
    {
        public int Id { get; set; }
        public int TicketNumber { get; set; }
        [Display(Name = "Data Backup")]
        public bool NeedsBackup { get; set; }

        // related members from Customer
        public int CustomerId { get; set; }
        public string CustomerFName { get; set; }
        public string CustomerLName { get; set; }

        // related members from Device
        public int DeviceId { get; set; }
        public string DeviceMake { get; set; }
        public string DeviceModelNumber { get; set; }
        public string DeviceOS { get; set; }

        // related members from Log
        [Display(Name = "Date Opened")]
        public DateTime LogOpenDate { get; set; }
        [Display(Name = "Status")]
        public TicketStatus LogLatestStatus { get; set; }
        [Display(Name = "Last Worked On")]
        public DateTime LogLatestDate { get; set; }
        [Display(Name = "Date Closed")]
        public DateTime LogCloseDate { get; set; }

        // for date filtering
        public DateFilter DateFilter { get; set; }
    }
}
