using CSMWebCore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface IReportsService
    {
        /// <summary>
        /// Gets count of active tickets, TicketStatus optional parameter
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        int TotalActiveTickets(TicketStatus? status = null);
        /// <summary>
        /// Gets count of completed tickets, TimeSpan optional parameter
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        int TotalCompletedTickets(TimeSpan? span = null);
        /// <summary>
        /// Gets count of completed tickets between dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        int TotalCompletedTickets(DateTime startDate, DateTime endDate);
        /// <summary>
        /// Returns count of Checked in tickets, TimeSpan optional parameter
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        int TotalCheckedInTickets(TimeSpan? span = null);
        /// <summary>
        /// Returns count of Checked in tickets between two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        int TotalCheckedInTickets(DateTime startDate, DateTime endDate);
        /// <summary>
        /// Returns count of Checked out tickets, timespan optional parameter
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        int TotalCheckedOutTickets(TimeSpan? span = null);
        /// <summary>
        /// Returns count of checked out tickets between two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        int TotalCheckedOutTickets(DateTime startDate, DateTime endDate);
        /// <summary>
        /// Gets count of contact logs by a given user, timespan optional parameter
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        int GetContactLogsByUser(string userName, TimeSpan? span = null);
        /// <summary>
        /// Gets count of contact logs by a given user between two dates
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        int GetContactLogsByUser(string userName, DateTime startDate, DateTime endDate);
        /// <summary>
        /// Gets count of service logs by a given user, timespan optional parameter
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        int GetServiceLogsByUser(string userName, TimeSpan? span = null);
        /// <summary>
        /// Gets count of service logs by a given user between two dates
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        int GetServiceLogsByUser(string userName, DateTime startDate, DateTime endDate);
        /// <summary>
        /// Gets count of consultations, timespan optional parameter
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        int GetConsultations(TimeSpan? span = null);
        /// <summary>
        /// Gets count of consultations between two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        int GetConsultations(DateTime startDate, DateTime endDate);
        /// <summary>
        /// Gets count of consultations by a given user, timespan optional parameter
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        int GetConsultationsLogsByUser(string userName, TimeSpan? span = null);
        /// <summary>
        /// Gets count of consultations by user between two dates
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        int GetConsultationsLogsByUser(string userName, DateTime startDate, DateTime endDate);
        /// <summary>
        /// Generates a TicketProgressReport for the given ticketId
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        //-----------Ticket Progress Report
        TicketProgressReport PrintProgressReport(int ticketId);
        /// <summary>
        /// Gets total amount of savings by ticketId
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        decimal GetSavingsByTicket(int ticketId);
        /// <summary>
        /// Gets total savings for all tickets.  Optional timespan parameter
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        decimal GetTicketSavingsOverTimePeriod(TimeSpan? span = null);
        /// <summary>
        /// Gets the total savings for all tickets between two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        decimal GetTicketSavingsOverTimePeriod(DateTime startDate, DateTime endDate);
        /// <summary>
        /// Gets the total savings for all consultations.  Optional timespan parameter
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        decimal GetConsultSavingsOverTimePeriod(TimeSpan? span = null);
        /// <summary>
        /// Gets the total consultation savings between two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        decimal GetConsultSavingsOverTimePeriod(DateTime startDate, DateTime endDate);
        /// <summary>
        /// Gets the average handle time (from checkedin to finished) of tickets completed within
        /// the given timeSpan.  Returns TimeSpan.Zero if no tickets were completed in the 
        /// TimeSpan.
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        TimeSpan GetAverageHandleTime(TimeSpan span);
    }
}
