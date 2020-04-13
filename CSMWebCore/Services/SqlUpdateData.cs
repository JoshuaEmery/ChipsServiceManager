using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;

namespace CSMWebCore.Services
{
    public class SqlUpdateData : IUpdateData
    {
        private ChipsDbContext _db;
        public SqlUpdateData(ChipsDbContext db)
        {
            _db = db;
        }
        public void Add(TicketProgress update)
        {
            _db.Updates.Add(update);
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }

        public TicketProgress Get(Guid id)
        {
            return _db.Find<TicketProgress>(id);
        }

        public int GetTicketId(Guid id)
        {
            return _db.Find<TicketProgress>(id).TicketId;
        }
    }
}
