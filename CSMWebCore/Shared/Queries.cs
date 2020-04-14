using CSMWebCore.Entities;
using CSMWebCore.Shared;
using CSMWebCore.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CSMWebCore.Shared
{
    public static class Queries
    {
        // ================ General ================

        /// <summary>
        /// Gets a single entity object from DbSet<TEntity> where TEntity implements IHasId
        /// </summary>
        public static IHasId GetById<TEntity>(this DbSet<TEntity> dbSet, int id) where TEntity : class, IHasId => dbSet.Find(id);

        /// <summary>
        /// Gets a collection of entity objects, with filtering and ordering by lambda expressions.
        /// includeProperties is a comma-separated string for specifying related entities, which
        /// allows the query to return entity properties related to the top-level entity to force
        /// a wider scope.
        /// </summary>
        public static IEnumerable<TEntity> Get<TEntity>(
            this DbSet<TEntity> dbSet,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "") where TEntity : class
        {
            IQueryable<TEntity> query = dbSet;

            // filter query given a function delegate or lambda expression
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // call include to bring related entities into the query (for example,
            // if calling from the customer repository and needing related devices)
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // order by a query given a function delegate or lambda expression
            // that takes an IQueryable and returns an IOrderedQueryable
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }


        // ================ Customers ================

        /// <summary>
        /// Searches relevant Customer fields for a matching search value and 
        /// returns a collection of matching Customers.
        /// </summary>
        public static List<Customer> Search(this DbSet<Customer> dbSet, string searchValue)
        {
            var result = new List<Customer>();
            if (!string.IsNullOrEmpty(searchValue))
            {
                result.AddRange(dbSet.Where(c => c.FirstName.Contains(searchValue)));
                result.AddRange(dbSet.Where(c => c.LastName.Contains(searchValue)));
                result.AddRange(dbSet.Where(c => c.Phone.Contains(searchValue)));
                result.AddRange(dbSet.Where(c => c.StudentId.Contains(searchValue)));
                result.AddRange(dbSet.Where(c => c.Email.Contains(searchValue)));
            }
            return result;
        }


        // ================ Devices ================

        /// <summary>
        /// Gets a collection of Devices for a given Customer ID.
        /// </summary>
        public static IQueryable<Device> GetDevicesByCustId(this DbSet<Device> dbSet, int customerId) =>
            dbSet.Where(x => x.Customer.Id == customerId);

        /// <summary>
        /// Searches relevant Device fields for a matching search value and
        /// returns a collection of matching Devices.
        /// </summary>
        public static List<Device> Search(this DbSet<Device> dbSet, string searchValue)
        {
            var result = new List<Device>();
            if (!string.IsNullOrEmpty(searchValue))
            {
                result.AddRange(dbSet.Where(d => d.Make.Contains(searchValue)));
                result.AddRange(dbSet.Where(d => d.ModelNumber.Contains(searchValue)));
                result.AddRange(dbSet.Where(d => d.OperatingSystem.Contains(searchValue)));
                result.AddRange(dbSet.Where(d => d.Password.Contains(searchValue)));
            }
            return result;
        }


        // ================ Tickets ================

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
        /// Gets all tickets with the given TicketStatus its latest status.
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


        // ----------------- Get By Status - Currently Open -----------------

        /// <summary>
        /// Gets a collection of Tickets where the current status (found in the last log) 
        /// is open (anything but Closed).
        /// </summary>
        // This has been tested to work. OrderByDescending is used with FirstOrDefault, rather than just LastOrDefault
        // because LastOrDefault cannot be easily converted into a query by EF and throws an error at runtime:
        // "The LINQ expression could not be translated".
        public static IQueryable<Ticket> GetOpenTickets(this DbSet<Ticket> dbSet) =>
            dbSet.Where(t => t.Logs.OrderByDescending(log => log.Id).FirstOrDefault().TicketStatus != TicketStatus.Closed);


        // ----------------- Get By Status - Checked In -----------------

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


        // ----------------- Get By Status - Completed (pending pickup) -----------------

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


        // ----------------- Get By Status - Closed -----------------

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


        // ================ Logs ================

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


        // ================ Events ================

        /// <summary>
        /// Gets the Check-In event object.
        /// </summary>
        public static Event GetCheckInEvent(this DbSet<Event> events) => 
            (Event)events.GetById(1);

        /// <summary>
        /// Gets the Check-Out event object.
        /// </summary>
        public static Event GetCheckOutEvent(this DbSet<Event> events) => 
            (Event)events.GetById(2);

        /// <summary>
        /// Gets a collection of Events that belong to a certain category.
        /// </summary>
        public static IQueryable<Event> GetEventsByCategory(this DbSet<Event> events, EventCategory category) => 
            events.Where(e => e.Category == category);

        /// <summary>
        /// Gets a collection of distinct Events that were found for logs of a 
        /// given Ticket ID. Duplicate events are excluded.
        /// </summary>
        public static IQueryable<Event> GetDistinctEventsByTicketId(this DbSet<Log> dbSet, int ticketId) =>
            dbSet.Where(x => x.TicketId == ticketId).Select(x => x.Event).Distinct();
    }
}
