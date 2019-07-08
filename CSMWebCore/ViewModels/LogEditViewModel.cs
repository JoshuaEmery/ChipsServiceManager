using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class LogEditViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TicketId { get; set; }
        public DateTime Logged { get; set; }
        public string Notes { get; set; }
        public LogType LogType { get; set; }
        public ContactMethod GetContactMethod { get; set; }
    }
}
