using CSMWebCore.Data;
using CSMWebCore.Entities;
using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public class SqlTicketsHistoryData : ITicketsHistoryData
    {
        private ChipsDbContext _db;
        public SqlTicketsHistoryData(ChipsDbContext db)
        {
            _db = db;
        }

        public IEnumerable<TicketHistory> GetAll()
        {
            return _db.TicketsHistory;
        }
        //method that takes a ticket, adds it to the history table and returns
        //the newly generated primary key from the tickethistory table.
        public int AddTicketToHistory(Ticket ticket)
        {
            TicketHistory ticketHistory = new TicketHistory
            {
                TicketId = ticket.Id,
                CheckedIn = ticket.CheckInDate,
                CheckedOut = ticket.CheckOutDate,
                CheckInUserId = ticket.CheckInUserId,
                CheckOutUserId = ticket.CheckOutUserId,
                DeviceId = ticket.DeviceId,
                Finished = ticket.FinishDate,
                NeedsBackup = ticket.NeedsBackup,
                TicketNumber = ticket.TicketNumber,
                TicketStatus = ticket.TicketStatus,
                AddedToHistory = DateTime.Now
            };
            _db.Add(ticketHistory);
            _db.SaveChanges();
            return ticketHistory.Id;
        }
        //Method that takes a ticket object and returns a TicketProgressReport for that ticket
        public TicketProgressReport GetTicketProgressReport(Ticket ticket)
        {
            //create a new ticket progress report
            TicketProgressReport ticketProgressReport = new TicketProgressReport();
            //create a timespan array
            TimeSpan[] timeByStatus = new TimeSpan[5];
            //get the list of tickethistory entries for this ticket and create a list from it so
            //it can be accessed by index
            List<TicketHistory> ticketHistories = _db.TicketsHistory.Where(x => x.TicketId == ticket.Id).ToList();
            //assign the id
            ticketProgressReport.TicketId = ticket.Id;
            //if there are no entries in tickethistory then the status is still new so the time is simply
            //the difference between today and checkin
            if (ticketHistories.Count == 0)
            {
                ticketProgressReport.TicketProgress.Add(TicketStatus.New, DateTime.Now - ticket.CheckInDate);
                return ticketProgressReport;
            }
            //for loop to iterate through tickethistories list
            for (int i = 0; i < ticketHistories.Count; i++)
            {
                //if it is the first entry in the tickethistories list
                if (i == 0)
                {
                    //add time to the timebyStatus array at the index of the status in the current tickethistories
                    //item.  The amount of time is the difference between when this log was made and the ticket checked in
                    timeByStatus[(int)ticketHistories[i].TicketStatus] += ticketHistories[i].AddedToHistory - ticketHistories[i].CheckedIn;
                }
                //any other entries in the tickethistories list
                else
                {
                    //add time to the timebyStatus array at the index of the status in the current tickethistories
                    //item.  The amount of time is the difference between when this log was made and the previous log
                    //was made.
                    timeByStatus[(int)ticketHistories[i].TicketStatus] += ticketHistories[i].AddedToHistory - ticketHistories[i - 1].AddedToHistory;
                }
            }
            //iterate through the timeByStatus array and if there is
            //a time at the given index add both the index(as a ticketStatus)
            //and the amount of time to the ticketprogress dictionary
            for (int i = 0; i < timeByStatus.Length; i++)
            {
                if (timeByStatus[i] > TimeSpan.Zero)
                {
                    ticketProgressReport.TicketProgress.Add((TicketStatus)i, timeByStatus[i]);
                }
            }
            return ticketProgressReport;
        }
        public IEnumerable<TicketProgressReport> GetTicketProgressReports(TimeSpan? span)
        {
            List<TicketProgressReport> result = new List<TicketProgressReport>();
            if (!span.HasValue)
            {
                foreach (var ticketHistory in _db.TicketsHistory)
                {

                    result.Add(GetTicketProgressReport(_db.Tickets.Find(ticketHistory.TicketId)));
                }
            }
            else
            {
                DateTime date = (DateTime.Now - span.Value);
                IEnumerable<TicketHistory> ticketHistories = _db.TicketsHistory.Where(x => x.AddedToHistory > date);
                foreach (var ticketHistory in ticketHistories)
                {

                    result.Add(GetTicketProgressReport(_db.Tickets.Find(ticketHistory.TicketId)));
                }
            }
            return result;
        }
        public IEnumerable<TicketProgressReport> GetTicketProgressReports(DateTime startDate, DateTime endDate)
        {
            List<TicketProgressReport> result = new List<TicketProgressReport>();
            IEnumerable<TicketHistory> ticketHistories = _db.TicketsHistory.Where(x => x.AddedToHistory > startDate && x.AddedToHistory < endDate);
            foreach (var ticketHistory in ticketHistories)
            {

                result.Add(GetTicketProgressReport(_db.Tickets.Find(ticketHistory.TicketId)));
            }

            return result;
        }
    }
}
