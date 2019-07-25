using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;
using CSMWebCore.Models;

namespace CSMWebCore.Services
{
    public class SqlLogData : ILogData
    {
        private ChipsDbContext _db;
        public SqlLogData(ChipsDbContext db)
        {
            _db = db;
        }
        public void Add(Log log)
        {
            _db.Add(log);
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }

        public Log Get(int id)
        {
            return _db.Find<Log>(id);
        }

        public IEnumerable<Log> GetAll()
        {
            return _db.Logs;
        }
        public Log GetLastByTicketId(int ticketId)
        {
            return _db.Find<Log>(_db.Logs.Where(x => x.TicketId == ticketId).Max(y => y.Id));
        }
        public IEnumerable<Log> GetLogsByTicketId(int ticketId)
        {
            return _db.Logs.Where(x => x.TicketId == ticketId);
        }
        public IEnumerable<Log> GetServiceLogsByTicketId(int ticketId)
        {
            return _db.Logs.Where(log => log.TicketId == ticketId && log.ContactMethod == ContactMethod.NoContact);
        }
        public IEnumerable<Log> GetContactLogsByTicketId(int ticketId)
        {
            return _db.Logs.Where(log => log.TicketId == ticketId && log.ContactMethod != ContactMethod.NoContact);
        }

    }
}
