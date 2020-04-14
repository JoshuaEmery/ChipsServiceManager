using CSMWebCore.Entities;
using CSMWebCore.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Shared
{
    public static class LogQueries
    {
        /// <summary>
        /// Gets the last Log for a given Ticket ID by finding largest Log ID.
        /// </summary>
        public static Log GetLatestLogByTicketId(this DbSet<Log> dbSet, int ticketId) =>
            dbSet.Find(dbSet.Where(x => x.TicketId == ticketId).Max(y => y.Id));

        /// <summary>
        /// Gets a collection of Logs for a given Ticket ID.
        /// </summary>
        public static IQueryable<Log> GetLogsByTicketId(this DbSet<Log> dbSet, int ticketId) =>
            dbSet.Where(x => x.TicketId == ticketId);


        // --------------- Service Logs ---------------

        // private utility method for determining if a log's event is a general, hardware, or software service
        private static bool IsService(Log log)
        {
            EventCategory category = log.Event.Category;
            if (category == EventCategory.GeneralService || category == EventCategory.HWService || category == EventCategory.SWService) return true;
            return false;
        }

        /// <summary>
        /// Gets a collection of Logs with a service event (one of three 
        /// categories: General Service, Hardware Service, or Software Service) for a given Ticket ID.
        /// </summary>
        public static IQueryable<Log> GetServiceLogsByTicketId(this DbSet<Log> dbSet, int ticketId) =>
            dbSet.Where(log => log.TicketId == ticketId && IsService(log));

        /// <summary>
        /// Gets a collection of Logs with a service event (one of three 
        /// categories: General Service, Hardware Service, or Software Service) for a given username,
        /// and between two dates.
        /// </summary>
        public static IQueryable<Log> GetServiceLogsByUser(this DbSet<Log> dbSet, string userName, DateTime startDate, DateTime endDate) =>
            dbSet.Where(log => log.UserCreated == userName && IsService(log) && log.DateCreated >= startDate && log.DateCreated < endDate);

        /// <summary>
        /// Gets a collection of Logs with a service event (one of three 
        /// categories: General Service, Hardware Service, or Software Service) for a given username,
        /// and optionally within the given TimeSpan (the end interval is today at midnight).
        /// </summary>
        public static IQueryable<Log> GetServiceLogsByUser(this DbSet<Log> dbSet, string userName, TimeSpan? span = null)
        {
            if (!span.HasValue) return dbSet.Where(log => log.UserCreated == userName && IsService(log));
            else
            {
                // gets span with end bound set to midnight tonight, to avoid overlap between business days
                DateTime end = DateTime.Today.AddDays(1);
                DateTime start = end.Subtract(span.Value);
                return GetServiceLogsByUser(dbSet, userName, start, end);
            }
        }


        // --------------- Contact Logs ---------------

        /// <summary>
        /// Gets a collection of Logs with a service event (one of three 
        /// categories: General Service, Hardware Service, or Software Service) for a given Ticket ID.
        /// </summary>
        public static IQueryable<Log> GetContactLogsByTicketId(this DbSet<Log> dbSet, int ticketId) =>
            dbSet.Where(log => log.TicketId == ticketId && log.Event.Category == EventCategory.Contact);

        /// <summary>
        /// Gets a collection of Logs with a service event (one of three 
        /// categories: General Service, Hardware Service, or Software Service) for a given username,
        /// and between two dates.
        /// </summary>
        public static IQueryable<Log> GetContactLogsByUser(this DbSet<Log> dbSet, string userName, DateTime startDate, DateTime endDate) =>
            dbSet.Where(log => log.UserCreated == userName && log.Event.Category == EventCategory.Contact && log.DateCreated >= startDate && log.DateCreated < endDate);

        /// <summary>
        /// Gets a collection of Logs with a service event (one of three 
        /// categories: General Service, Hardware Service, or Software Service) for a given username,
        /// and optionally within the given TimeSpan (the end interval is today at midnight).
        /// </summary>
        public static IQueryable<Log> GetContactLogsByUser(this DbSet<Log> dbSet, string userName, TimeSpan? span = null)
        {
            if (!span.HasValue) return dbSet.Where(log => log.UserCreated == userName && log.Event.Category == EventCategory.Contact);
            else
            {
                // gets span with end bound set to midnight tonight, to avoid overlap between business days
                DateTime end = DateTime.Today.AddDays(1);
                DateTime start = end.Subtract(span.Value);
                return GetContactLogsByUser(dbSet, userName, start, end);
            }
        }
    }
}
