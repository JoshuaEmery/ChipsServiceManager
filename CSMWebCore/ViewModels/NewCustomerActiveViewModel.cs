using CSMWebCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class NewCustomerActiveViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [Display(Name = "Student ID")]
        public string StudentId { get; set; }
        [Display(Name = "Contact Preference")]
        public ContactPref ContactPref { get; set; }
    }
}
