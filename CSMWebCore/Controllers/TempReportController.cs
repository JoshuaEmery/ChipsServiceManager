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
    //This to be deleted after report with Ty on Tuesday
    public class TempReportController : Controller
    {
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;
        private ITicketsHistoryData _ticketsHistory;
        private IConsultationData _consultations;
        private IServicePriceData _servicePrices;
        private readonly UserManager<ChipsUser> _userManager;

        public TempReportController(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs, 
            ITicketsHistoryData ticketsHistory, IConsultationData consultations, 
            IServicePriceData servicePrices ,UserManager<ChipsUser> userManager)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
            _ticketsHistory = ticketsHistory;
            _consultations = consultations;
            _userManager = userManager;
            _servicePrices = servicePrices;
        }
        //Quick testing index method that just creates a string response
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
            output += TotalCompletedTickets(new DateTime(2020,01,01),DateTime.Now);
            output += TotalCheckedInTickets();
            output += TotalCheckedInTickets(new TimeSpan(7, 0, 0, 0));
            output += TotalCheckedInTickets(new TimeSpan(30, 0, 0, 0));
            output += TotalCheckedInTickets(new TimeSpan(90, 0, 0, 0));
            output += TotalCheckedInTickets(new TimeSpan(365, 0, 0, 0));
            output += TotalCheckedInTickets(new DateTime(2020, 01, 01), DateTime.Now);
            output += TotalCheckedOutTickets();
            output += TotalCheckedOutTickets(new TimeSpan(7, 0, 0, 0));
            output += TotalCheckedOutTickets(new TimeSpan(30, 0, 0, 0));
            output += TotalCheckedOutTickets(new TimeSpan(90, 0, 0, 0));
            output += TotalCheckedOutTickets(new TimeSpan(365, 0, 0, 0));
            output += TotalCheckedOutTickets(new DateTime(2020, 01, 01), DateTime.Now);
            foreach (var ticket in _tickets.GetAll())
            {
                output += PrintProgressReport(ticket.Id);
            }
            output += GetConsultations();
            output += GetConsultations(new TimeSpan(7, 0, 0, 0));
            output += GetConsultations(new TimeSpan(30, 0, 0, 0));
            output += GetConsultations(new TimeSpan(90, 0, 0, 0));
            output += GetConsultations(new TimeSpan(365, 0, 0, 0));
            output += GetConsultations(new DateTime(2020, 01, 01), DateTime.Now);
            foreach (var user in _userManager.Users.ToList())
            {
                output += GetContactLogsByUser(user.UserName);
                output += GetContactLogsByUser(user.UserName, new TimeSpan(30, 0, 0, 0));
                output += GetContactLogsByUser(user.UserName, new TimeSpan(90, 0, 0, 0));
                output += GetContactLogsByUser(user.UserName, new DateTime(2020, 01, 01), DateTime.Now);
                output += GetServiceLogsByUser(user.UserName);
                output += GetServiceLogsByUser(user.UserName, new TimeSpan(30, 0, 0, 0));
                output += GetServiceLogsByUser(user.UserName, new TimeSpan(90, 0, 0, 0));
                output += GetServiceLogsByUser(user.UserName, new DateTime(2020, 01, 01), DateTime.Now);
                output += GetConsultationsLogsByUser(user.UserName);
                output += GetConsultationsLogsByUser(user.UserName, new TimeSpan(30, 0, 0, 0));
                output += GetConsultationsLogsByUser(user.UserName, new TimeSpan(90, 0, 0, 0));
                output += GetConsultationsLogsByUser(user.UserName, new DateTime(2020, 01, 01), DateTime.Now);
            }
            foreach (var ticket in _tickets.GetAll())
            {
                output += GetSavingsByTicket(ticket.Id);
            }
            output += GetTicketSavingsOverTimePeriod(new TimeSpan(7, 0, 0, 0));
            output += GetTicketSavingsOverTimePeriod(new TimeSpan(30, 0, 0, 0));
            output += GetTicketSavingsOverTimePeriod(new TimeSpan(90, 0, 0, 0));
            output += GetTicketSavingsOverTimePeriod();
            output += GetTicketSavingsOverTimePeriod(new DateTime(2020, 01, 01), DateTime.Now);
            output += GetConsultSavingsOverTimePeriod(new TimeSpan(7, 0, 0, 0));
            output += GetConsultSavingsOverTimePeriod(new TimeSpan(30, 0, 0, 0));
            output += GetConsultSavingsOverTimePeriod(new TimeSpan(90, 0, 0, 0));
            output += GetConsultSavingsOverTimePeriod();
            output += GetConsultSavingsOverTimePeriod(new DateTime(2020, 01, 01), DateTime.Now);
            return Content(output);
        }
        private string TotalActiveTickets(TicketStatus? status = null)
        {

            if (!status.HasValue)
                return $"Total Active Tickets: {_tickets.GetAllActiveTickets().Count()}\n";
            else
                return $"{status.ToString()}: {_tickets.GetByStatus(status.Value).Count()}\n";
        }

        private string TotalCompletedTickets(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return $"Tickets Completed all time: {_tickets.GetAllCompletedTickets().Count()}\n";
            else
                return $"Tickets Completed in the last {span.Value.TotalDays} days {_tickets.GetCompletedTickets(span.Value).Count()}\n";            
        }
        private string TotalCompletedTickets(DateTime startDate, DateTime endDate)
        {
            return $"Tickets Completed between {startDate} and {endDate} {_tickets.GetCompletedTickets(startDate, endDate).Count()}\n";
        }
        private string TotalCheckedInTickets(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return $"Tickets Checked in all time: {_tickets.GetAll().Count()}\n";
            else
                return $"Tickets Checked in last {span.Value.TotalDays} days {_tickets.GetCheckedInTickets(span.Value).Count()}\n";
        }
        private string TotalCheckedInTickets(DateTime startDate, DateTime endDate)
        {
            return $"Tickets Checked in between {startDate} and {endDate} {_tickets.GetCheckedInTickets(startDate, endDate).Count()}\n";
        }
        private string TotalCheckedOutTickets(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return $"Tickets Checked out all time: {_tickets.GetAll().Count()}\n";
            else
                return $"Tickets Checked out last {span.Value.TotalDays} days {_tickets.GetCheckedOutTickets(span.Value).Count()}\n";
        }
        private string TotalCheckedOutTickets(DateTime startDate, DateTime endDate)
        {            
            return $"Tickets Checked out between {startDate} and {endDate} {_tickets.GetCheckedOutTickets(startDate,endDate).Count()}\n";
        }
        //-----------------User Reports
        private string GetContactLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return $"Total Contacts By {userName}: {_logs.GetContactLogsByUser(userName).Count()}\n";
            else
                return $"Total Contacts By {userName} in last {span.Value.TotalDays}: {_logs.GetContactLogsByUser(userName, span.Value).Count()}\n";
        }
        private string GetContactLogsByUser(string userName, DateTime startDate, DateTime endDate)
        {
            return $"Total Contacts By {userName} between {startDate} and {endDate}: {_logs.GetContactLogsByUser(userName, startDate, endDate).Count()}\n";
        }
        private string GetServiceLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return $"Total Service By {userName}: {_logs.GetServiceLogsByUser(userName).Count()}\n";
            else
                return $"Total Service By {userName} in last {span.Value.TotalDays}: {_logs.GetServiceLogsByUser(userName, span.Value).Count()}\n";
        }
        private string GetServiceLogsByUser(string userName, DateTime startDate, DateTime endDate)
        {
            return $"Total Service By {userName} between {startDate} and {endDate}: {_logs.GetServiceLogsByUser(userName, startDate, endDate).Count()}\n";
        }
        //-----------Consultations completed 
        private string GetConsultations(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return $"Consultations Completed all time: {_consultations.GetAll().Count()}\n";
            else
                return $"Consultations Completed in the last {span.Value.TotalDays} days {_consultations.GetConsultations(span.Value).Count()}\n";
        }
        private string GetConsultations(DateTime startDate, DateTime endDate)
        {
            return $"Consultations Completed between {startDate} and {endDate}: {_consultations.GetConsultations(startDate, endDate).Count()}\n";
        }
        //-----------Consultations completed by user
        private string GetConsultationsLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return $"Total Consultations By {userName}: {_consultations.GetConsultationsByUser(userName).Count()}\n";
            else
                return $"Total Consultations By {userName} int the last {span.Value.TotalDays} days: {_consultations.GetConsultationsByUser(userName, span.Value).Count()}\n";
        }
        private string GetConsultationsLogsByUser(string userName, DateTime startDate, DateTime endDate)
        {
                return $"Total Consultations By {userName} between {startDate} and {endDate} days: {_consultations.GetConsultationsByUser(userName, startDate, endDate).Count()}\n";
        }
        
        //-----------Ticket Progress Report
        private string PrintProgressReport(int ticketId)
        {
            string output = "";
            TicketProgressReport tpr = _ticketsHistory.GetTicketProgressReport(_tickets.Get(ticketId));
            output += $"Ticket number {tpr.TicketId} time spent in each category:\n";            
            foreach (var item in tpr.TicketProgress)
            {
                output += $"{item.Key} - {item.Value}\n";
            }            
            return output;
        }
        //------------Financial Reports
        private string GetSavingsByTicket(int ticketId)
        {
            string output = "";
            output += $"The total cost for ticketId: " +
                $"{_tickets.Get(ticketId).TicketNumber} " +
                $"{_servicePrices.GetTotalPrice(_logs.GetDistinctLogTypesByTicketId(ticketId))}\n";
            return output;
        }
        private string GetTicketSavingsOverTimePeriod(TimeSpan? span = null)
        {
            IEnumerable<Ticket> tickets;            
            if (!span.HasValue)
            {
                tickets = _tickets.GetAll();                
            }
            else
            {
                tickets = _tickets.GetCheckedInTickets(span.Value);
            }
            decimal total = 0m;           
            foreach (var ticket in tickets)
            {
                total += _servicePrices.GetTotalPrice(_logs.GetDistinctLogTypesByTicketId(ticket.Id));
            }
            
            string output = "";
            string timePeriod = (span.HasValue) ? span.Value.Days.ToString() : "All Time";
            output += $"The total ticket savings over last {timePeriod}: {total:C2}\n";
            return output;
        }
        private string GetTicketSavingsOverTimePeriod(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Ticket> tickets = _tickets.GetCheckedInTickets(startDate, endDate);
            decimal total = 0m;
            foreach (var ticket in tickets)
            {
                total += _servicePrices.GetTotalPrice(_logs.GetDistinctLogTypesByTicketId(ticket.Id));
            }
            string output = "";
            output += $"The total ticket savings between {startDate} and {endDate}: {total:C2}\n";
            return output;
        }
        private string GetConsultSavingsOverTimePeriod(TimeSpan? span = null)
        {
            IEnumerable<Consultation> consults;
            if (!span.HasValue)
            {
                consults = _consultations.GetAll();
            }
            else
            {
                consults = _consultations.GetConsultations(span.Value);
            }
            decimal total = 0m;
            string output = "";
            string timePeriod = (span.HasValue) ? span.Value.Days.ToString() : "All Time";
            total += _servicePrices.GetPriceOfServiceType(LogType.Diagnostic) * consults.Count();
            output += $"The total consultation savings over last {timePeriod}: {total:C2}\n";
            return output;
        }
        private string GetConsultSavingsOverTimePeriod(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Consultation> consults = _consultations.GetConsultations(startDate, endDate);
            decimal total = 0m;
            string output = "";
            total += _servicePrices.GetPriceOfServiceType(LogType.Diagnostic) * consults.Count();
            output += $"The total consultation savings between {startDate} and {endDate}: {total:C2}\n";
            return output;
        }
    }
}