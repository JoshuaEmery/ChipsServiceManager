using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;

namespace CSMWebCore.Services
{
    public class XSqlUpdateData : XIUpdateData
    {
        private ChipsDbContext _db;
        public XSqlUpdateData(ChipsDbContext db)
        {
            _db = db;
        }
        public void Add(XUpdate update)
        {
            _db.Updates.Add(update);
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }

        public XUpdate Get(Guid id)
        {
            return _db.Find<XUpdate>(id);
        }

        public int GetTicketId(Guid id)
        {
            return _db.Find<XUpdate>(id).TicketId;
        }
    }
}
