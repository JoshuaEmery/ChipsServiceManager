using CSMWebCore.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class NewDeviceEditViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string ModelNumber { get; set; }
        [Display(Name = "Operating System")]
        public string OperatingSystem { get; set; }
        public string Password { get; set; }
        [Display(Name = "Has the computer been serviced before?")]
        public bool Serviced { get; set; }
    }
}
