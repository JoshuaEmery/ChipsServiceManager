using CSMWebCore.Entities;
using CSMWebCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface IServicePriceData
    {
        decimal GetTotalPriceOfTicket(Ticket ticket);
        decimal GetPriceOfLogEvent(Event e);
    }
}
