using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class UpdateViewModel
    {
        public Ticket Ticket { get; set; }
        public Device Device { get; set; }
        public Log Log { get; set; }
        public Customer Customer { get; set; }
        public Update Update { get; set; }


    }
}
