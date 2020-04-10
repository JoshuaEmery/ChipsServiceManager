using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Models
{
    public enum TicketStatus
    {
        New,
        [Display(Name = "In Progress")]
        InProgress,
        [Display(Name = "Needs Contact")]
        NeedsContact,
        [Display(Name = "Pending Response")]
        PendingResponse,
        [Display(Name = "Pending Pickup")]
        PendingPickup,
        Closed
    }
}
