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
        Ticket GetById(int id);
        void Add(Ticket ticket);
        int Commit();
        IEnumerable<Ticket> GetByStatus(TicketStatus status);
        int GetLatestTicketNum();
        IEnumerable<Ticket> GetAllByDevice(int deviceId);
        IEnumerable<Ticket> GetOpen();
        IEnumerable<Ticket> Search(string searchValue);
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
