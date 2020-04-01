using CSMWebCore.Entities;
using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface ITicketsHistoryData
    {
        IEnumerable<TicketHistory> GetAll();
        int AddTicketToHistory(Ticket ticket);
        TicketProgressReport GetTicketProgressReport(Ticket ticket);
    }
}
