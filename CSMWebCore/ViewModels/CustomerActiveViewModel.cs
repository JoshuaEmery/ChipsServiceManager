using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class CustomerActiveViewModel
    {
        public Ticket Ticket { get; set; }
        public Customer Customer { get; set; }
        public Device Device { get; set; }
    }
}
