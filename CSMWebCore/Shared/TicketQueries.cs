using CSMWebCore.Entities;
using CSMWebCore.Enums;
using CSMWebCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Shared
{
    public static class TicketQueries
    {
        // ----------------- General -----------------

        /// <summary>
        /// Gets a collection of Tickets for a given Device ID.
        /// </summary>
        public static IQueryable<Ticket> GetTicketsByDeviceId(this DbSet<Ticket> dbSet, int deviceId) =>
            dbSet.Where(t => t.Device.Id == deviceId);

        /// <summary>
        /// Gets the most recently created Ticket for a given Device ID.
        /// </summary>
        public static Ticket GetLatestTicketForDevice(this DbSet<Ticket> dbSet, int deviceId) =>
            dbSet.Where(t => t.Device.Id == deviceId).LastOrDefault();

        /// <summary>
        /// Gets all tickets with the given TicketStatus as its latest status.
        /// </summary>
        public static IQueryable<Ticket> GetTicketsByLatestStatus(this DbSet<Ticket> dbSet, TicketStatus status) =>
            dbSet.Where(t => t.Logs.LastOrDefault().TicketStatus == status);

        /// <summary>
        /// Gets the latest ticket number assigned to a Ticket. If no Tickets
        /// are found in the database or the operation fails, 0 is returned.
        /// </summary>
        public static int GetLatestTicketNum(this DbSet<Ticket> dbSet)
        {
            try
            {
                return dbSet.Max(t => t.TicketNumber);
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets a running total of the cost of all services found in each Log's Event.
        /// </summary>
        public static decimal GetTotalPriceOfTicket(this Ticket ticket)
        {
            decimal total = 0.00m;
            IEnumerable<Log> logs = ticket.Logs;
            foreach (Log log in logs)
            {
                total += log.Event.Price;
            }
            return total;
        }

        /// <summary>
        /// Searches relevant Device fields for a matching search value and
        /// returns a collection of matching Devices.
        /// </summary>
        public static List<Ticket> Search(this DbSet<Ticket> dbSet, string searchValue)
        {
            var result = new List<Ticket>();
            if (!String.IsNullOrEmpty(searchValue))
            {
                result.AddRange(dbSet.Where(c => c.CheckInDate.ToShortDateString().Contains(searchValue)));
                result.AddRange(dbSet.Where(c => c.CheckOutDate.ToShortDateString().Contains(searchValue)));
                result.AddRange(dbSet.Where(c => c.CheckInUserId.Contains(searchValue)));
                result.AddRange(dbSet.Where(c => c.CheckOutUserId.Contains(searchValue)));
                result.AddRange(dbSet.Where(c => c.TicketNumber.ToString().Contains(searchValue)));
            }
            return result;
        }


        // ----------------- Tickets By Status - Currently Open -----------------

        /// <summary>
        /// Gets a collection of Tickets where the current status (found in the last log) 
        /// is open (anything but Closed).
        /// </summary>
        // This has been tested to work. OrderByDescending is used with FirstOrDefault, rather than just LastOrDefault
        // because LastOrDefault cannot be easily converted into a query by EF and throws an error at runtime:
        // "The LINQ expression could not be translated".
        public static IQueryable<Ticket> GetOpenTickets(this DbSet<Ticket> dbSet) =>
            dbSet.Where(t => t.Logs.OrderByDescending(log => log.Id).FirstOrDefault().TicketStatus != TicketStatus.Closed);


        // ----------------- Tickets By Status - Checked In -----------------

        /// <summary>
        /// Gets a collection of Tickets that were opened (device checked-in) within the given date range.
        /// "Checked-in" Tickets are selected by checking all Logs of each Ticket for EventEnum.CheckIn.
        /// </summary>
        public static IQueryable<Ticket> GetCheckedInTickets(this DbSet<Ticket> dbSet, DateTime startDate, DateTime endDate) =>
            dbSet.Where(t => t.Logs.FirstOrDefault(log => log.EventId == (int)EventEnum.CheckIn).DateCreated >= startDate && t.CheckInDate < endDate);

        /// <summary>
        /// Gets a collection of Tickets that were opened (device checked-in), optionally within the 
        /// given TimeSpan. "Checked-in" Tickets are selected by checking the Logs of each Ticket for 
        /// EventEnum.CheckIn. The first log found with a check-in event is used for checking date bounds.
        /// </summary>
        public static IQueryable<Ticket> GetCheckedInTickets(this DbSet<Ticket> dbSet, TimeSpan? span)
        {
            if (!span.HasValue) return dbSet.Where(t => t.Logs.FirstOrDefault().EventId == (int)EventEnum.CheckIn);
            else
            {
                // gets span with end bound set to midnight tonight, to avoid overlap between business days
                DateTime end = DateTime.Today.AddDays(1);
                DateTime start = end.Subtract(span.Value);
                return GetCheckedInTickets(dbSet, start, end);
            }
        }


        // ----------------- Tickets By Status - Completed (pending pickup) -----------------

        /// <summary>
        /// Gets a collection of Tickets that were considered completed within the given date range.
        /// Completed tickets are selected by checking Logs of each Ticket for TicketStatus.PendingPickup.
        /// </summary>
        public static IQueryable<Ticket> GetCompletedTickets(this DbSet<Ticket> dbSet, DateTime startDate, DateTime endDate) =>
            dbSet.Where(t => t.Logs.Any(log => log.TicketStatus == TicketStatus.PendingPickup && log.DateCreated >= startDate && log.DateCreated < endDate));

        /// <summary>
        /// Gets a collection of Tickets that were considered completed, optionally within the
        /// given TimeSpan (the end interval is today at midnight). Completed Tickets are selected by 
        /// checking Logs of each Ticket for TicketStatus.PendingPickup.
        /// </summary>
        public static IQueryable<Ticket> GetCompletedTickets(this DbSet<Ticket> dbSet, TimeSpan? span = null)
        {
            if (!span.HasValue) return dbSet.Where(t => t.Logs.Any(log => log.TicketStatus == TicketStatus.PendingPickup));
            else
            {
                // gets span with end bound set to midnight tonight, to avoid overlap between business days
                DateTime end = DateTime.Today.AddDays(1);
                DateTime start = end.Subtract(span.Value);
                return GetCompletedTickets(dbSet, start, end);
            }
        }


        // ----------------- Tickets By Status - Closed -----------------

        /// <summary>
        /// Gets a collection of Tickets that are closed, whether via device check-out or other means, within the given date range. 
        /// Closed tickets are selected by checking the final Log of each Ticket for TicketStatus.Closed.
        /// </summary>
        public static IQueryable<Ticket> GetClosedTickets(this DbSet<Ticket> dbSet, DateTime startDate, DateTime endDate) =>
            dbSet.GetTicketsByLatestStatus(TicketStatus.Closed).Where(t => t.Logs.LastOrDefault().DateCreated >= startDate && t.Logs.LastOrDefault().DateCreated < endDate);

        /// <summary>
        /// Gets a collection of Tickets that are closed, whether via device check-out or other means, optionally within a given TimeSpan (the end interval is today at midnight). 
        /// Closed tickets are selected by checking the final Log of each Ticket for TicketStatus.Closed.
        /// </summary>
        // * This was rewritten and needs to be tested
        public static IQueryable<Ticket> GetClosedTickets(this DbSet<Ticket> dbSet, TimeSpan? span = null)
        {
            if (!span.HasValue) return dbSet.GetTicketsByLatestStatus(TicketStatus.Closed);
            else
            {
                // gets span with end bound set to midnight tonight, to avoid overlap between business days
                DateTime end = DateTime.Today.AddDays(1);
                DateTime start = end.Subtract(span.Value);
                return GetClosedTickets(dbSet, start, end);
            }
        }


    }
}
