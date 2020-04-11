using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Entities
{
    public class Device : Entity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string Make { get; set; }
        public string  ModelNumber { get; set; }
        public string OperatingSystem { get; set; }
        public string Password { get; set; }
        public bool Serviced { get; set; }
    }
}
