using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Enums
{
    public enum TicketStatus
    {
        New = 0,
        [Display(Name = "In Progress")]
        InProgress = 1,
        [Display(Name = "Needs Contact")]
        NeedsContact = 2,
        [Display(Name = "Pending Response")]
        PendingResponse = 3,
        [Display(Name = "Pending Pickup")]
        PendingPickup = 4,
        Closed = 5
    }
}
