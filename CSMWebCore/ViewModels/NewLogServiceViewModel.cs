using CSMWebCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class NewLogServiceViewModel
    {
        public int TicketNumber { get; set; }
        public int TicketId { get; set; }
        public string LogNotes { get; set; }
        [Display(Name = "Service Type")]
        public int SelectedEventId { get; set; }
        public IEnumerable<SelectListItem> Events { get; set; }
        public TicketStatus TicketStatus { get; set; }

    }
}
