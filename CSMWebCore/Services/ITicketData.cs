using CSMWebCore.Entities;
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
    }
}
