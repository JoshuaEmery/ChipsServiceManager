using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class HomeIndexViewModel
    {
        public int newCount { get; set; }
        public int inProgressCount { get; set; }
        public int pendingResponseCount { get; set; }
        public int pendingPickupCount { get; set; }
        public TimeSpan avgAge { get; set; }
        public TimeSpan maxAge { get; set; }
        public int maxAgeTicketId { get; set; }
        public TimeSpan avgIdle { get; set; }
        public TimeSpan maxIdle { get; set; }
        public TimeSpan weekAvgHandle { get; set; }
        public TimeSpan monthAvgHandle { get; set; }
        public TimeSpan ninetyDayAvgHandle { get; set; }
        public TimeSpan yearAvgHangle { get; set; }
        public int maxIdleTicketId { get; set; }
        


    }
}
