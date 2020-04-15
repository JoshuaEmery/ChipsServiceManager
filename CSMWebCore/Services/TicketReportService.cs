using CSMWebCore.Data;
using CSMWebCore.Shared;
using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Models;
using CSMWebCore.Enums;

namespace CSMWebCore.Services
{
    public class TicketReportService : ITicketReportService
    {
        private ChipsDbContext context;
        public TicketReportService(ChipsDbContext context)
        {
            this.context = context;
        }
        public TicketProgressReport GetTicketProgressReport(Ticket ticket)
        {
            var logs = context.Logs.GetLogsByTicketId(ticket.Id).ToList();
            //create a new ticket progress report
            TicketProgressReport ticketProgressReport = new TicketProgressReport();
            ticketProgressReport.TicketId = ticket.Id;
            //create a timespan array
            TimeSpan[] timeByStatus = new TimeSpan[5];
            //interate through the logs
            for (int i = 0; i < logs.Count() - 1; i++)
            {
                timeByStatus[(int)logs[i].TicketStatus] += logs[i + 1].DateCreated - logs[i].DateCreated;
                    
            }
            for (int i = 0; i < timeByStatus.Length; i++)
            {
                ticketProgressReport.TicketProgress.Add((TicketStatus)i, timeByStatus[i]);
            }
            return ticketProgressReport;
        }
    }
}
