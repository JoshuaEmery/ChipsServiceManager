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
    //Implementation of ITicketData
    public class SqlTicketData : ITicketData
    {
        //db context and injection
        private ChipsDbContext _db;
        public SqlTicketData(ChipsDbContext db)
        {
            _db = db;
        }

        //track ticket entity for later saving to db
        public void Add(Ticket ticket)
        {
            _db.Add(ticket);
        }

        //save to db (returns # of entries written)
        public int Commit() => _db.SaveChanges();

        //get all tickets of a given status
        public IEnumerable<Ticket> GetByStatus(TicketStatus status) => _db.Tickets.Where(x => x.TicketStatus == status);

        //get latest ticket number
        public int GetLatestTicketNum()
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
        //get ticket by ID
        public Ticket GetById(int id) => _db.Find<Ticket>(id);

        //get all tickets
        public IEnumerable<Ticket> GetAll() => _db.Tickets;

        //get all open tickets
        public IEnumerable<Ticket> GetOpen() => _db.Tickets.Where(x => x.TicketStatus != TicketStatus.Closed);

        //get all closed tickets
        public IEnumerable<Ticket> GetCompleted() => GetByStatus(TicketStatus.Closed);

        //get all tickets for a given device
        public IEnumerable<Ticket> GetAllByDevice(int deviceId) => _db.Tickets.Where(x => x.DeviceId == deviceId);

        //get most recent ticket for a device
        public Ticket GetLatestForDevice(int deviceId) => _db.Find<Ticket>(_db.Tickets.Where(x => x.DeviceId == deviceId).LastOrDefault().Id);

        //method that gets all tickets that have been completed within a timespan        
        //public IEnumerable<Ticket> GetCompleted(TimeSpan span)
        //{
        //    DateTime date = (DateTime.Now - span);
        //    return _db.Tickets.Where(x => x.FinishDate > date);
        //}
        // TODO overflow method that takes a span and "snaps" to days rather than a 

        //get tickets closed within date range        
        public IEnumerable<Ticket> GetCompleted(DateTime startDate, DateTime endDate) => _db.Tickets.Where(x => x.FinishDate > startDate && x.FinishDate < endDate);

        //get closed tickets 
        public IEnumerable<Ticket> GetCompleted(TimeSpan span)
        {
            // get midnight tomorrow as end date
            DateTime end = DateTime.Today.AddDays(1);
            DateTime start = end.Subtract(span);
            return GetCompleted(start, end);
        }

        
        //get tickets checked in within a timespan
        public IEnumerable<Ticket> GetAll(TimeSpan span)
        {
            DateTime date = (DateTime.Now - span);
            return _db.Tickets.Where(x => x.CheckInDate > date);
        }
        //get tickets checked in between two dates
        public IEnumerable<Ticket> GetAll(DateTime startDate, DateTime endDate)
        {
            return _db.Tickets.Where(x => x.CheckInDate > startDate && x.CheckInDate < endDate);
        }
        //get tickets checked out within a timespan
        public IEnumerable<Ticket> GetClosed(TimeSpan span)
        {
            DateTime date = (DateTime.Now - span);
            return _db.Tickets.Where(x => x.CheckOutDate > date);
        }
        //get tickets checked out between two dates
        public IEnumerable<Ticket> GetClosed(DateTime startDate, DateTime endDate)
        {
            return _db.Tickets.Where(x => x.CheckOutDate > startDate && x.CheckOutDate < endDate);
        }

        public IEnumerable<Ticket> Search(string searchValue)
        {
            var result = new List<Ticket>();
            if (!String.IsNullOrEmpty(searchValue))
            {
                result.AddRange(_db.Tickets.Where(c => c.CheckInDate.ToShortDateString().Contains(searchValue)));
                result.AddRange(_db.Tickets.Where(c => c.CheckOutDate.ToShortDateString().Contains(searchValue)));
                result.AddRange(_db.Tickets.Where(c => c.CheckInUserId.Contains(searchValue)));
                result.AddRange(_db.Tickets.Where(c => c.CheckOutUserId.Contains(searchValue)));
                result.AddRange(_db.Tickets.Where(c => c.TicketNumber.ToString().Contains(searchValue)));
            }
            return result;

        }


    }
}
