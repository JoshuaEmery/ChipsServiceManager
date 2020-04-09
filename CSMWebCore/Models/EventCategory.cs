using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Models
{
    public enum EventCategory
    {
        General = 0,
        [Display(Name = "Software")]
        SWService = 1,
        [Display(Name = "Hardware")]
        HWService = 2,
        Contact = 3
    }
}
