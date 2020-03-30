using CSMWebCore.Data;
using CSMWebCore.Entities;
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
                CheckedIn = ticket.CheckedIn,
                CheckedOut = ticket.CheckedOut,
                CheckInUserId = ticket.CheckInUserId,
                CheckOutUserId = ticket.CheckOutUserId,
                DeviceId = ticket.DeviceId,
                Finished = ticket.Finished,
                NeedsBackup = ticket.NeedsBackup,
                TicketNumber = ticket.TicketNumber,
                TicketStatus = ticket.TicketStatus,
                AddedToHistory = DateTime.Now
            };
            _db.Add(ticketHistory);
            _db.SaveChanges();
            return ticketHistory.Id;
        }
    }
}
