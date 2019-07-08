using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;

namespace CSMWebCore.Services
{
    public class SqlTicketData : ITicketData
    {
        private ChipsDbContext _db;
        public SqlTicketData(ChipsDbContext db)
        {
            _db = db;
        }
        public void Add(Ticket ticket)
        {
            _db.Add(ticket);
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }

        public Ticket Get(int id)
        {
            return _db.Find<Ticket>(id);
        }

        public IEnumerable<Ticket> GetAll()
        {
            return _db.Tickets;
        }
    }
}
