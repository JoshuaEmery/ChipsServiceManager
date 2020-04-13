using CSMWebCore.Data;
using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public class ConsultationRepository : GenericRepository<Consultation>, IConsultationRepository
    {
        public ConsultationRepository(ChipsDbContext db) : base(db)
        { }

        public IEnumerable<Consultation> GetConsultations(TimeSpan span)
        {
            DateTime date = (DateTime.Now - span);
            return context.Consultations.Where(x => x.Time > date);
        }
        public IEnumerable<Consultation> GetConsultations(DateTime startDate, DateTime endDate)
        {
            return context.Consultations.Where(x => x.Time > startDate && x.Time < endDate);
        }

        public IEnumerable<Consultation> GetConsultationsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
            {
                return context.Consultations.Where(x => x.UserName == userName);
            }
            DateTime date = (DateTime.Now - span.Value);
            return context.Consultations.Where(x => x.UserName == userName && x.Time > date);
        }
        public IEnumerable<Consultation> GetConsultationsByUser(string userName, DateTime startDate, DateTime endDate)
        {
            return context.Consultations.Where(x => x.UserName == userName && x.Time > startDate && x.Time < endDate);
        }
    }
}
