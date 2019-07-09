using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class LogViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TicketId { get; set; }
        public string Logged { get; set; }
        public string Notes { get; set; }
        public string LogType { get; set; }
        public string ContactMethod { get; set; }
    }
}
