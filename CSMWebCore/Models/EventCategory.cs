using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Models
{
    public enum EventCategory
    {
        OpenTicket = 0,
        CloseTicket = 1,
        [Display(Name = "General")]
        GeneralService = 2,
        [Display(Name = "Software")]
        SWService = 3,
        [Display(Name = "Hardware")]
        HWService = 4,
        Contact = 5
    }
}
