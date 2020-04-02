using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class DeviceViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Customer")]
        public Customer Customer { get; set; }
        public Ticket Ticket { get; set; }
        public string Make { get; set; }
        public string ModelNumber { get; set; }
        [Display(Name = "OS")]
        public string OperatingSystem { get; set; }
        public string Password { get; set; }
        public bool Serviced { get; set; }
    }
}
