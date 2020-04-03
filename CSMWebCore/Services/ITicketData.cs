using CSMWebCore.Entities;
using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface ITicketData
    {
        IEnumerable<Ticket> GetAll();
        Ticket Get(int id);
        void Add(Ticket ticket);
        int Commit();
        IEnumerable<Ticket> GetByStatus(TicketStatus status);
        int CurrentTicketNumber();
        IEnumerable<Ticket> GetAllByDevice(int deviceId);
        IEnumerable<Ticket> GetAllActiveTickets();
        IEnumerable<Ticket> Search(string searchValue);
        int CountByStatus(TicketStatus status);
        IEnumerable<Ticket> GetCheckedInTickets(DateTime startDate, DateTime endDate);
        IEnumerable<Ticket> GetCheckedInTickets(TimeSpan span);
        IEnumerable<Ticket> GetCheckedOutTickets(DateTime startDate, DateTime endDate);
        IEnumerable<Ticket> GetCheckedOutTickets(TimeSpan span);
        IEnumerable<Ticket> GetCompletedTickets(DateTime startDate, DateTime endDate);
        IEnumerable<Ticket> GetCompletedTickets(TimeSpan span);

        IEnumerable<Ticket> GetAllCompletedTickets();
        Ticket GetRecentByDevice(int deviceId);


    }

}
