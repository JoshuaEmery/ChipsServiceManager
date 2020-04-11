using CSMWebCore.Entities;
using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface ILogRepository : IRepository<Log>
    {
        Log GetById(int id);
        Log GetLastByTicketId(int ticketId);
        IEnumerable<Log> GetLogsByTicketId(int ticketId);
        IEnumerable<LogType> GetDistinctLogTypesByTicketId(int ticketId);
        IEnumerable<Log> GetContactLogsByTicketId(int ticketId);
        IEnumerable<Log> GetServiceLogsByTicketId(int ticketId);
        IEnumerable<Log> GetServiceLogsByUser(string userId, TimeSpan? span = null);
        IEnumerable<Log> GetServiceLogsByUser(string userId, DateTime startDate, DateTime endDate);
        IEnumerable<Log> GetContactLogsByUser(string userId, TimeSpan? span = null);
        IEnumerable<Log> GetContactLogsByUser(string userId, DateTime startDate, DateTime endDate);
    }
}
