using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class NewCustomerViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [Display(Name = "Student ID")]
        public string StudentId { get; set; }
        [Display(Name = "Contact Preference")]
        public string ContactPref { get; set; }
    }
}
