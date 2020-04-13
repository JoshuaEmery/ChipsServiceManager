using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class TicketDeviceCustCreateVM
    {
        // ticket
        public int Id { get; set; }
        public int TicketNumber { get; set; }
        [Display(Name = "Needs Data Backup")]
        public bool NeedsBackup { get; set; }
        // device
        public string Make { get; set; }
        public string ModelNumber { get; set; }
        public string OperatingSystem { get; set; }
        public string Password { get; set; }
        public bool Serviced { get; set; }
        // customer
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string StudentId { get; set; }
        [Display(Name = "Contact Preference")]
        public ContactPref ContactPref { get; set; }

        // initial log
        [Display(Name = "Technician")]
        public string UserCreated { get; set; }
        [Required(ErrorMessage = "Please enter information about the problem.")]
        [Display(Name  = "Initial Note")]
        // notes are only required on some types of events
        public string Notes { get; set; }

        // controller also sets:
        // - Log.DateCreated to current datetime
        // - Log.TicketStatus to TicketStatus.New
        // - and Log.Event to check-in event
    }
}
