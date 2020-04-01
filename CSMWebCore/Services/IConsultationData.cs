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
        IEnumerable<Consultation> GetConsultationsWithinTimeSpan(TimeSpan span);
        IEnumerable<Consultation> GetContactLogsByUserandTime(string userName, TimeSpan? span = null);
    }
}
