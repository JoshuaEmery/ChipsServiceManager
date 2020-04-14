using CSMWebCore.Entities;
using CSMWebCore.Enums;
using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Shared
{
    public static class TicketProgressReportQueries
    {
        // * these three methods need to be reworked to use a ticket's logs rather than ticket histories

        /// <summary>
        /// Return a TicketProgressReport given a ticketId
        /// </summary>
        public static TicketProgressReport GetTicketProgressReport(this Ticket ticket)
        {
            //create a new ticket progress report
            TicketProgressReport ticketProgressReport = new TicketProgressReport();
            //create a timespan array
            TimeSpan[] timeByStatus = new TimeSpan[5];
            //get the list of tickethistory entries for this ticket and create a list from it so
            //it can be accessed by index
            //List<TicketHistory> ticketHistories = _db.TicketsHistory.Where(x => x.TicketId == ticket.Id).ToList();
            //assign the id
            ticketProgressReport.TicketId = ticket.Id;
            //if there are no entries in tickethistory then the status is still new so the time is simply
            //the difference between today and checkin
            //if (ticketHistories.Count == 0)
            //{
            //    ticketProgressReport.TicketProgress.Add(TicketStatus.New, DateTime.Now - ticket.CheckInDate);
            //    return ticketProgressReport;
            //}
            ////for loop to iterate through tickethistories list
            //for (int i = 0; i < ticketHistories.Count; i++)
            //{
            //    //if it is the first entry in the tickethistories list
            //    if (i == 0)
            //    {
            //        //add time to the timebyStatus array at the index of the status in the current tickethistories
            //        //item.  The amount of time is the difference between when this log was made and the ticket checked in
            //        timeByStatus[(int)ticketHistories[i].TicketStatus] += ticketHistories[i].AddedToHistory - ticketHistories[i].CheckedIn;
            //    }
            //    //any other entries in the tickethistories list
            //    else
            //    {
            //        //add time to the timebyStatus array at the index of the status in the current tickethistories
            //        //item.  The amount of time is the difference between when this log was made and the previous log
            //        //was made.
            //        timeByStatus[(int)ticketHistories[i].TicketStatus] += ticketHistories[i].AddedToHistory - ticketHistories[i - 1].AddedToHistory;
            //    }
            //}
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

        /// <summary>
        /// Return a list of TicketProgressReports with nullable Timepsan Parameter
        /// </summary>
        //public static IEnumerable<TicketProgressReport> GetTicketProgressReports(TimeSpan? span)
        //{
        //    List<TicketProgressReport> result = new List<TicketProgressReport>();
        //    if (!span.HasValue)
        //    {
        //        foreach (var ticketHistory in _db.TicketsHistory)
        //        {

        //            result.Add(GetTicketProgressReport(_db.Tickets.Find(ticketHistory.TicketId)));
        //        }
        //    }
        //    else
        //    {
        //        DateTime date = (DateTime.Now - span.Value);
        //        IEnumerable<TicketHistory> ticketHistories = _db.TicketsHistory.Where(x => x.AddedToHistory > date);
        //        foreach (var ticketHistory in ticketHistories)
        //        {

        //            result.Add(GetTicketProgressReport(_db.Tickets.Find(ticketHistory.TicketId)));
        //        }
        //    }
        //    return result;
        //}

        /// <summary>
        /// Return a list of TicketProgressReports for tickets between startdate and enddate
        /// </summary>
        //public static IEnumerable<TicketProgressReport> GetTicketProgressReports(DateTime startDate, DateTime endDate)
        //{
        //    List<TicketProgressReport> result = new List<TicketProgressReport>();
        //    IEnumerable<TicketHistory> ticketHistories = _db.TicketsHistory.Where(x => x.AddedToHistory > startDate && x.AddedToHistory < endDate);
        //    foreach (var ticketHistory in ticketHistories)
        //    {

        //        result.Add(GetTicketProgressReport(_db.Tickets.Find(ticketHistory.TicketId)));
        //    }

        //    return result;
        //}
    }
}
