using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;
using CSMWebCore.Enums;

namespace CSMWebCore.Services
{
    public class LogRepository : GenericRepository<Log>, ILogRepository
    {
        public LogRepository(ChipsDbContext db) : base(db)
        { }

        public Log GetLastByTicketId(int ticketId)
        {
            return context.Logs.Find(context.Logs.Where(x => x.TicketId == ticketId).Max(y => y.Id));
        }
        public IEnumerable<Log> GetLogsByTicketId(int ticketId)
        {
            return context.Logs.Where(x => x.TicketId == ticketId);
        }
        //Method that takes a ticketId and returns all of the Unique LogType values that
        //ticket had performed on it
        public IEnumerable<LogType> GetDistinctLogTypesByTicketId(int ticketId)
        {            
            return context.Logs.Where(x => x.TicketId == ticketId).Select(x => x.LogType).Distinct();
        }

        public IEnumerable<Log> GetServiceLogsByTicketId(int ticketId)
        {
            return context.Logs.Where(log => log.TicketId == ticketId && log.ContactMethod == ContactMethod.NoContact);
        }
        public IEnumerable<Log> GetContactLogsByTicketId(int ticketId)
        {
            return context.Logs.Where(log => log.TicketId == ticketId && log.ContactMethod != ContactMethod.NoContact);
        }
        public IEnumerable<Log> GetServiceLogsByUser(string userId, TimeSpan? span = null)
        {
            if (!span.HasValue)
            {
                return context.Logs.Where(log => log.UserCreated == userId && log.ContactMethod == ContactMethod.NoContact);
            }
            DateTime date = (DateTime.Now - span.Value);
            return context.Logs.Where(log => log.UserCreated == userId  && log.ContactMethod == ContactMethod.NoContact
            && log.DateCreated > date);
        }
        public IEnumerable<Log> GetServiceLogsByUser(string userId, DateTime startDate, DateTime endDate)
        {
             return context.Logs.Where(log => log.UserCreated == userId && log.ContactMethod == ContactMethod.NoContact
            && log.DateCreated > startDate && log.DateCreated < endDate);
        }
        public IEnumerable<Log> GetContactLogsByUser(string userId, TimeSpan? span = null)
        {
            if(!span.HasValue)
            {
                return context.Logs.Where(log => log.UserCreated == userId && log.ContactMethod != ContactMethod.NoContact);
            }
            DateTime date = (DateTime.Now - span.Value);
            return context.Logs.Where(log => log.UserCreated == userId  && log.ContactMethod != ContactMethod.NoContact
            && log.DateCreated > date);
        }
        public IEnumerable<Log> GetContactLogsByUser(string userId, DateTime startDate, DateTime endDate)
        {
            return context.Logs.Where(log => log.UserCreated == userId && log.ContactMethod != ContactMethod.NoContact
           && log.DateCreated > startDate && log.DateCreated < endDate);
        }

    }
}
