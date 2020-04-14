using CSMWebCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Entities
{
    public class ServicePrice
    {
        public int Id { get; set; }
        public LogType Service { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
    }
}
