using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Models
{
    public enum TicketResult
    {
        Fixed,
        NotFixedPerClient,
        NotFixedBrokenBeyondRepair,
        NotFixedLackingTools
    }
}
