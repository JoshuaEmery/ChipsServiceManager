using CSMWebCore.Entities;
using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        IEnumerable<Ticket> Search(string searchValue);
        IEnumerable<Ticket> GetByStatus(TicketStatus status);
        int GetLatestTicketNum();
        IEnumerable<Ticket> GetAllByDevice(int deviceId);
        IEnumerable<Ticket> GetOpen();

        IEnumerable<Ticket> GetAll(DateTime startDate, DateTime endDate);
        IEnumerable<Ticket> GetAll(TimeSpan span);
        IEnumerable<Ticket> GetClosed(DateTime startDate, DateTime endDate);
        IEnumerable<Ticket> GetClosed(TimeSpan span);
        IEnumerable<Ticket> GetCompleted(DateTime startDate, DateTime endDate);
        IEnumerable<Ticket> GetCompleted(TimeSpan span);

        IEnumerable<Ticket> GetCompleted();
        Ticket GetLatestForDevice(int deviceId);


    }

}
