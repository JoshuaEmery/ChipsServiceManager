using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface ILogData
    {
        IEnumerable<Log> GetAll();
        Log Get(int id);
        void Add(Log log);
        int Commit();
        Log GetLastByTicketId(int ticketId);
        IEnumerable<Log> GetLogsByTicketId(int ticketId);
        IEnumerable<Log> GetContactLogsByTicketId(int ticketId);
        IEnumerable<Log> GetServiceLogsByTicketId(int ticketId);
        IEnumerable<Log> GetServiceLogsByUserandTime(string userId, TimeSpan? span = null);
        IEnumerable<Log> GetContactLogsByUserandTime(string userId, TimeSpan? span = null);
    }
}
