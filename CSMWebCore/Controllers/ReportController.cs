using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Entities;
using CSMWebCore.Models;
using CSMWebCore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSMWebCore.Controllers
{
    //test1
    public class ReportController : Controller
    {
        // Kyle's conflicting comment
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;
        private ITicketsHistoryData _ticketsHistory;
        private readonly UserManager<ChipsUser> _userManager;

        public ReportController(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs, 
            ITicketsHistoryData ticketsHistory, UserManager<ChipsUser> userManager)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
            _ticketsHistory = ticketsHistory;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            string output = "";
            output += TotalActiveTickets();
            foreach (TicketStatus status in Enum.GetValues(typeof(TicketStatus)))
            {
                output += TotalActiveTickets(status);
            }
            output += TotalCompletedTickets();
            output += TotalCompletedTickets(new TimeSpan(7,0,0,0));
            output += TotalCompletedTickets(new TimeSpan(30, 0, 0, 0));
            output += TotalCompletedTickets(new TimeSpan(90, 0, 0, 0));
            output += TotalCompletedTickets(new TimeSpan(365, 0, 0, 0));
            output += TotalCheckedInTickets();
            output += TotalCheckedInTickets(new TimeSpan(7, 0, 0, 0));
            output += TotalCheckedInTickets(new TimeSpan(30, 0, 0, 0));
            output += TotalCheckedInTickets(new TimeSpan(90, 0, 0, 0));
            output += TotalCheckedInTickets(new TimeSpan(365, 0, 0, 0));
            foreach (var user in _userManager.Users.ToList())
            {
                output += GetTotalContactLogsByUser(user.UserName);
                output += GetTotalServiceLogsByUser(user.UserName);
            }
            return Content(output);
        }
        //-------------------Ticket Reports
        //Method that return values based on status or time etc... eventually these will just
        //return int but string is easier to work with for displaying in testing
        public string TotalActiveTickets(TicketStatus? status = null)
        {
            if (!status.HasValue)
                return $"Total Active Tickets: {_tickets.GetAllActiveTickets().Count()}\n";
            else
                return $"{status.ToString()}: {_tickets.GetByStatus(status.Value).Count()}\n";
        }

        public string TotalCompletedTickets(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return $"Completed all time: {_tickets.GetAllCompletedTickets().Count()}\n";
            else
                return $"Completed in the last {span.Value.TotalDays} days {_tickets.GetTicketsCompletedWithinTimeSpan(span.Value).Count()}\n";            
        }
        public string TotalCheckedInTickets(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return $"Checked in all time: {_tickets.GetAll().Count()}\n";
            else
                return $"Checked in last {span.Value.TotalDays} days {_tickets.GetTicketsCheckedInWithinTimeSpan(span.Value).Count()}\n";
        }
        //-----------------User Reports
        public string GetTotalContactLogsByUser(string userId)
        {
            return $"Total Contacts By {userId}: {_logs.GetContactLogsByUserandTime(userId).Count()}\n";
        }
        public string GetTotalServiceLogsByUser(string userId)
        {
            return $"Total Service By {userId}: {_logs.GetServiceLogsByUserandTime(userId).Count()}\n";
        }
    }
}