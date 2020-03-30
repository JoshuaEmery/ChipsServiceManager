using CSMWebCore.Data;
using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public class SqlConsultationData : IConsultationData
    {
        private ChipsDbContext _db;
        public SqlConsultationData(ChipsDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Consultation> GetAll()
        {
            return _db.Consultations;
        }

        public IEnumerable<Consultation> GetConsultationsWithinTimeSpan(TimeSpan span)
        {
            return _db.Consultations.Where(x => x.Time > DateTime.Now - span); ;
        }
    }
}
