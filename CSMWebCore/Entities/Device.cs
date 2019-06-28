using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Entities
{
    public class Device
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string OperatingSystem { get; set; }
        public string Password { get; set; }
        public bool Serviced { get; set; }

    }
}
