using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Models
{
    public enum TicketResult
    {
        Resolved,
        [Display(Name = "Unresolved - Customer Decision")]
        UnresolvedCustomerDecision,
        [Display(Name = "Unresolved - Broken Beyond Repair")]
        UnresolvedBrokenBeyondRepair,
        [Display(Name = "Unresolved - Lacking Tools")]
        UnresolvedLackingTools
    }
}
