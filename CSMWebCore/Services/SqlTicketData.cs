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
        //get the db context and inject into the constructor
        private ChipsDbContext _db;
        public SqlTicketData(ChipsDbContext db)
        {
            _db = db;
        }
        //Add a ticket to the database
        public void Add(Ticket ticket)
        {
            _db.Add(ticket);
        }
        //method that saves changes
        public int Commit()
        {
            return _db.SaveChanges();
        }
        //method that takes a ticketstatus and returns all tickets of that status
        public IEnumerable<Ticket> GetByStatus(TicketStatus status)
        {
            return _db.Tickets.Where(x => x.TicketStatus == status);
        }
        //method that takes a ticket status and returns the count of active tickets in that status
        public int CountByStatus(TicketStatus status)
        {
            return _db.Tickets.Where(x => x.TicketStatus == status).Count();
        }
        //method that gets the current ticket number
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
        //get ticket by ID
        public Ticket Get(int id)
        {
            return _db.Find<Ticket>(id);
        }
        //get all tickets
        public IEnumerable<Ticket> GetAll()
        {
            return _db.Tickets;
        }
        //get all active tickets
        public IEnumerable<Ticket> GetAllActiveTickets()
        {
            return _db.Tickets.Where(x => x.TicketStatus != TicketStatus.Done);
        }
        //get all tickets for a given device
        public IEnumerable<Ticket> GetAllByDevice(int deviceId)
        {
            return _db.Tickets.Where(x => x.DeviceId == deviceId);
        }
        //get most recent ticket for a device
        public Ticket GetRecentByDevice(int deviceId)
        {
            return _db.Find<Ticket>(_db.Tickets.Where(x => x.DeviceId == deviceId).LastOrDefault().Id);
        }
        //get all completed tickets
        public IEnumerable<Ticket> GetAllCompletedTickets()
        {
            return _db.Tickets.Where(x => x.TicketStatus == TicketStatus.Done);
        }
        //method that gets all tickets that have been completed within a timespan        
        public IEnumerable<Ticket> GetCompletedTickets(TimeSpan span)
        {
            // TODO make overflow for method which takes start/end datetime params
            DateTime date = (DateTime.Now - span);
            return _db.Tickets.Where(x => x.Finished > date);
        }
        //method that gets all tickets that have been completed between two dates        
        public IEnumerable<Ticket> GetCompletedTickets(DateTime startDate, DateTime endDate)
        {            
            return _db.Tickets.Where(x => x.Finished > startDate && x.Finished < endDate);
        }
        //method that returns all tickets checked in within a timespan
        public IEnumerable<Ticket> GetCheckedInTickets(TimeSpan span)
        {
            DateTime date = (DateTime.Now - span);
            return _db.Tickets.Where(x => x.CheckedIn > date);
        }
        //method that returns all tickets checked in between two dates
        public IEnumerable<Ticket> GetCheckedInTickets(DateTime startDate, DateTime endDate)
        {
            return _db.Tickets.Where(x => x.CheckedIn > startDate && x.CheckedIn < endDate);
        }
        //method that returns all tickets checked out within a timespan
        public IEnumerable<Ticket> GetCheckedOutTickets(TimeSpan span)
        {
            DateTime date = (DateTime.Now - span);
            return _db.Tickets.Where(x => x.CheckedOut > date);
        }
        //method that returns all tickets checked out between two dates
        public IEnumerable<Ticket> GetCheckedOutTickets(DateTime startDate, DateTime endDate)
        {
            return _db.Tickets.Where(x => x.CheckedOut > startDate && x.CheckedOut < endDate);
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
