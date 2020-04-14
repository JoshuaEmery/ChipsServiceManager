using CSMWebCore.Entities;
using CSMWebCore.Enums;
using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface ITicketsHistoryData
    {
        /// <summary>
        /// Get all entries in tickethistory table
        /// </summary>
        /// <returns></returns>
        IEnumerable<TicketHistory> Get();
        /// <summary>
        /// Add a ticket to the history table and return the id of the newly created tickethistory
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        int AddTicketToHistory(Ticket ticket);
        /// <summary>
        /// Return a TicketProgressReport given a ticketId
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        TicketProgressReport GetTicketProgressReport(Ticket ticket);
        /// <summary>
        /// Return a list of TicketProgressReports with nullable Timepsan Parameter
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        IEnumerable<TicketProgressReport> GetTicketProgressReports(TimeSpan? span);
        /// <summary>
        /// Return a list of TicketProgressReports for tickets between startdate and enddate
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<TicketProgressReport> GetTicketProgressReports(DateTime startDate, DateTime endDate);
    }
}
