using CSMWebCore.Entities;
using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class NewDeviceViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Customer")]
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public int TicketNumber { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public string Make { get; set; }
        public string ModelNumber { get; set; }
        [Display(Name = "OS")]
        public string OperatingSystem { get; set; }
        public string Password { get; set; }
        [Display(Name = "Prior Service")]
        public bool Serviced { get; set; }
    }
}
