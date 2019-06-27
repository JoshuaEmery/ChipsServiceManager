﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Models
{
    public enum TicketStatus
    {
        New,
        InProgress,
        PendingResponse,
        PendingPickup,
        Done
    }
}