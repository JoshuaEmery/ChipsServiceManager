using CSMWebCore.Entities;
using CSMWebCore.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Shared
{
    public static class EventQueries
    {
        /// <summary>
        /// Gets the Check-In event object.
        /// </summary>
        public static Event GetCheckInEvent(this DbSet<Event> events) =>
            (Event)events.Find(1);

        /// <summary>
        /// Gets the Check-Out event object.
        /// </summary>
        public static Event GetCheckOutEvent(this DbSet<Event> events) =>
            (Event)events.Find(2);

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
