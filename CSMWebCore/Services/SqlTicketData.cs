using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;
using CSMWebCore.Models;
using CSMWebCore.Services;

namespace CSMWebCore.Services
{
    public class SqlTicketData : ITicketData
    {
        private ChipsDbContext _db;
        public SqlTicketData(ChipsDbContext db)
        {
            _db = db;
        }
        public void Add(Ticket ticket)
        {
            _db.Add(ticket);
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }

        public int CountByStatus(TicketStatus status)
        {
            return _db.Tickets.Where(x => x.TicketStatus == status).Count();
        }

        public int CurrentTicketNumber()
        {            
            try
            {
                return _db.Tickets.Max(t => t.TicketNumber);
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
        }

        public Ticket Get(int id)
        {
            return _db.Find<Ticket>(id);
        }

        public IEnumerable<Ticket> GetAll()
        {
            return _db.Tickets;
        }


        public IEnumerable<Ticket> GetAllActiveTickets()
        {
            return _db.Tickets.Where(x => x.TicketStatus != TicketStatus.Done);
        }

        public IEnumerable<Ticket> GetAllByDevice(int deviceId)
        {
            return _db.Tickets.Where(x => x.DeviceId == deviceId);
        }
        public Ticket GetRecentByDevice(int deviceId)
        {
            return _db.Find<Ticket>(_db.Tickets.Where(x => x.DeviceId == deviceId).LastOrDefault().Id);
        }

        public IEnumerable<Ticket> GetAllCompletedTickets()
        {
            return _db.Tickets.Where(x => x.TicketStatus == TicketStatus.Done);
        }

        public IEnumerable<Ticket> GetByStatus(TicketStatus status)
        {
            return _db.Tickets.Where(x => x.TicketStatus == status);
        }

        public IEnumerable<Ticket> GetTicketsCompletedWithinTimeSpan(TimeSpan span)
        {
            return _db.Tickets.Where(x => x.Finished > DateTime.Now - span);
        }

        public IEnumerable<Ticket> Search(string searchValue)
        {
            var result = new List<Ticket>();
            if (!String.IsNullOrEmpty(searchValue))
            {
                result.AddRange(_db.Tickets.Where(c => c.CheckedIn.ToShortDateString().Contains(searchValue)));
                result.AddRange(_db.Tickets.Where(c => c.CheckedOut.ToShortDateString().Contains(searchValue)));
                result.AddRange(_db.Tickets.Where(c => c.CheckInUserId.Contains(searchValue)));
                result.AddRange(_db.Tickets.Where(c => c.CheckOutUserId.Contains(searchValue)));
                result.AddRange(_db.Tickets.Where(c => c.TicketNumber.ToString().Contains(searchValue)));
            }
            return result;

        }
    }
}
