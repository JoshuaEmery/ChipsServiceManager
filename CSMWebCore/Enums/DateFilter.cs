using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Enums
{
    public enum DateFilter
    {
        Newest = 0,
        Oldest = 1,
        [Display(Name = "Oldest Log (Open)")]
        Idle = 2
    }
}
