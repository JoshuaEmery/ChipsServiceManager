using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;

namespace CSMWebCore.Services
{
    public class SqlLogData : ILogData
    {
        private ChipsDbContext _db;
        public SqlLogData(ChipsDbContext db)
        {
            _db = db;
        }
        public void Add(Log log)
        {
            _db.Add(log);
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }

        public Log Get(int id)
        {
            return _db.Find<Log>(id);
        }

        public IEnumerable<Log> GetAll()
        {
            return _db.Logs;
        }
    }
}
