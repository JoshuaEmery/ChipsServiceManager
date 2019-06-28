using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    interface ITicketData
    {
        List<Ticket> GetAll();
        Ticket Get(int id);
        void Add(Ticket ticket);
        int Commit();
    }
}
