using CSMWebCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Shared
{
    public static class ConsultationQueries
    {
        public static IEnumerable<Consultation> GetConsultations(this DbSet<Consultation> dbSet, TimeSpan span)
        {
            DateTime date = (DateTime.Now - span);
            return dbSet.Where(x => x.Time > date);
        }
        public static IEnumerable<Consultation> GetConsultations(this DbSet<Consultation> dbSet, DateTime startDate, DateTime endDate)
        {
            return dbSet.Where(x => x.Time > startDate && x.Time < endDate);
        }

        public static IEnumerable<Consultation> GetConsultationsByUser(this DbSet<Consultation> dbSet, string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
            {
                return dbSet.Where(x => x.UserName == userName);
            }
            DateTime date = (DateTime.Now - span.Value);
            return dbSet.Where(x => x.UserName == userName && x.Time > date);
        }
        public static IEnumerable<Consultation> GetConsultationsByUser(this DbSet<Consultation> dbSet, string userName, DateTime startDate, DateTime endDate)
        {
            return dbSet.Where(x => x.UserName == userName && x.Time > startDate && x.Time < endDate);
        }
    }
}
