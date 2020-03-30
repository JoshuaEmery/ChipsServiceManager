using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Entities
{
    public class Log
    {
        public int Id { get; set; }
        
        [Display(Name = "Technician")]
        public string UserId { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public DateTime Logged { get; set; }
        [Required(ErrorMessage = "Notes are required on all log entries")]        
        public string Notes { get; set; }
        public LogType LogType { get; set; }
        public ContactMethod ContactMethod { get; set; }
    }
}
