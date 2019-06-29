using CSMWebCore.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class DeviceEditViewModel
    {
        public int Id { get; set; }
        public List<SelectListItem> Customer { get; set; }
        public string CustomerId { get; set; }
        public string Make { get; set; }
        public string ModelNumber { get; set; }
        public string OperatingSystem { get; set; }
        public string Password { get; set; }
        public bool Serviced { get; set; }
    }
}
