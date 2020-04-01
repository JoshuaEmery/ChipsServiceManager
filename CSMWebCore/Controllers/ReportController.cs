﻿using System;
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
    public class ReportController : Controller
    {
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;
        private ITicketsHistoryData _ticketsHistory;
        private IConsultationData _consultations;
        private readonly UserManager<ChipsUser> _userManager;

        public ReportController(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs, 
            ITicketsHistoryData ticketsHistory, IConsultationData consultations, UserManager<ChipsUser> userManager)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
            _ticketsHistory = ticketsHistory;
            _consultations = consultations;
            _userManager = userManager;
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
            output += TotalCheckedInTickets();
            output += TotalCheckedInTickets(new TimeSpan(7, 0, 0, 0));
            output += TotalCheckedInTickets(new TimeSpan(30, 0, 0, 0));
            output += TotalCheckedInTickets(new TimeSpan(90, 0, 0, 0));
            output += TotalCheckedInTickets(new TimeSpan(365, 0, 0, 0));
            foreach (var ticket in _tickets.GetAll())
            {
                output += PrintProgressReport(ticket.Id);
            }
            output += GetTotalConsultations();
            output += GetTotalConsultations(new TimeSpan(7, 0, 0, 0));
            output += GetTotalConsultations(new TimeSpan(30, 0, 0, 0));
            output += GetTotalConsultations(new TimeSpan(90, 0, 0, 0));
            output += GetTotalConsultations(new TimeSpan(365, 0, 0, 0));
            foreach (var user in _userManager.Users.ToList())
            {
                output += GetTotalContactLogsByUser(user.UserName);
                output += GetTotalContactLogsByUser(user.UserName, new TimeSpan(30, 0, 0, 0));
                output += GetTotalContactLogsByUser(user.UserName, new TimeSpan(90, 0, 0, 0));
                output += GetTotalServiceLogsByUser(user.UserName);
                output += GetTotalServiceLogsByUser(user.UserName, new TimeSpan(30, 0, 0, 0));
                output += GetTotalServiceLogsByUser(user.UserName, new TimeSpan(90, 0, 0, 0));
                output += GetTotalConsultationsLogsByUser(user.UserName);
                output += GetTotalConsultationsLogsByUser(user.UserName, new TimeSpan(30, 0, 0, 0));
                output += GetTotalConsultationsLogsByUser(user.UserName, new TimeSpan(90, 0, 0, 0));
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
                return $"Tickets Completed all time: {_tickets.GetAllCompletedTickets().Count()}\n";
            else
                return $"Tickets Completed in the last {span.Value.TotalDays} days {_tickets.GetTicketsCompletedWithinTimeSpan(span.Value).Count()}\n";            
        }
        public string TotalCheckedInTickets(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return $"Tickets Checked in all time: {_tickets.GetAll().Count()}\n";
            else
                return $"Tickets Checked in last {span.Value.TotalDays} days {_tickets.GetTicketsCheckedInWithinTimeSpan(span.Value).Count()}\n";
        }
        //-----------------User Reports
        public string GetTotalContactLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return $"Total Contacts By {userName}: {_logs.GetContactLogsByUserandTime(userName).Count()}\n";
            else
                return $"Total Contacts By {userName} in last {span.Value.TotalDays}: {_logs.GetContactLogsByUserandTime(userName, span.Value).Count()}\n";
        }
        public string GetTotalServiceLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return $"Total Service By {userName}: {_logs.GetServiceLogsByUserandTime(userName).Count()}\n";
            else
                return $"Total Service By {userName} in last {span.Value.TotalDays}: {_logs.GetServiceLogsByUserandTime(userName, span.Value).Count()}\n";
        }
        //-----------Consultations completed 
        public string GetTotalConsultations(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return $"Consultations Completed all time: {_consultations.GetAll().Count()}\n";
            else
                return $"Consultations Completed in the last {span.Value.TotalDays} days {_consultations.GetConsultationsWithinTimeSpan(span.Value).Count()}\n";
        }
        //-----------Consultations completed by user
        public string GetTotalConsultationsLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return $"Total Consultations By {userName}: {_consultations.GetContactLogsByUserandTime(userName).Count()}\n";
            else
                return $"Total Consultations By {userName} int he last {span.Value.TotalDays} days: {_consultations.GetContactLogsByUserandTime(userName, span.Value).Count()}\n";
        }

        //-----------Ticket Progress Report
        public string PrintProgressReport(int ticketId)
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

    }
}