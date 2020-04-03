using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface IConsultationData
    {
        IEnumerable<Consultation> GetAll();       
        IEnumerable<Consultation> GetConsultations(TimeSpan span);
        IEnumerable<Consultation> GetConsultations(DateTime startDate, DateTime endDate);
        IEnumerable<Consultation> GetConsultationsByUser(string userName, TimeSpan? span = null);
        IEnumerable<Consultation> GetConsultationsByUser(string userName, DateTime startDate, DateTime endDate);

    }
}
