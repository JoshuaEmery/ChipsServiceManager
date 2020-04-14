using CSMWebCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Entities
{
    public class Event : IHasId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EventCategory Category { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
    }
}
