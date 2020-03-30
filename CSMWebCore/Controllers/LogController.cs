using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CSMWebCore.Entities;
using CSMWebCore.Models;
using CSMWebCore.Services;
using CSMWebCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSMWebCore.Controllers
{
    //check login
    [Authorize]
    //Logs table needs access to devices, customers, tickets, logs.  Also the Log table works primarily
    //by TicketID as that is the foreign key in the log table.
    public class LogController : Controller
    {
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;
        private ITicketsHistoryData _ticketsHistory;

        public LogController(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs, ITicketsHistoryData ticketsHistory)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
            _ticketsHistory = ticketsHistory;
        }
        //Test Actions
        //public IActionResult Index()
        //{
        //    var model = _logs.GetAll().Select(log => new LogViewModel
        //    {
        //        Id = log.Id,
        //        UserId = log.UserId,
        //        TicketId = log.TicketId,
        //        Logged = log.Logged.ToShortDateString(),
        //        Notes = log.Notes,
        //        LogType = log.LogType.ToString(),
        //        ContactMethod = log.ContactMethod.ToString()

        //    });
        //    return View(model);
        //}
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Create(LogEditViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Log log = new Log
        //        {
        //            UserId = User.FindFirst(ClaimTypes.Name).Value.ToString(),
        //            TicketId = model.TicketId,
        //            Logged = DateTime.Now,
        //            Notes = model.Notes,
        //            LogType = model.LogType,
        //            ContactMethod = model.ContactMethod
        //        };
        //        _logs.Add(log);
        //        _logs.Commit();
        //        return RedirectToAction("Index");
        //    }
        //    return View();

        //}
        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    var model = _logs.Get(id);
        //    if (model == null)
        //    {
        //        return View();
        //    }
        //    return View(model);
        //}
        //[HttpPost]
        //public IActionResult Edit(LogEditViewModel model)
        //{
        //    var log = _logs.Get(model.Id);
        //    if (log == null || !ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    log.UserId = model.UserId;
        //    log.TicketId = model.TicketId;
        //    log.Logged = model.Logged;
        //    log.Notes = model.Notes;
        //    log.LogType = model.LogType;
        //    log.ContactMethod = model.ContactMethod;
        //    _logs.Commit();
        //    return RedirectToAction("Index");
        //}
        //End Test Actions

        //Deployed Actions

        //Log/Service
        //Method that takes a ticketId and creates a service Log for that ticket
        [HttpGet]
        public IActionResult Service(int ticketId)
        {
            //get the ticket Object
            var ticket = _tickets.Get(ticketId);
            //create LogEditViewModel
            var model = new LogEditViewModel
            {
                TicketId = ticket.Id,
                TicketNumber = ticket.TicketNumber
            };
            return View(model);
        }
        //Post for /Log/Service
        [HttpPost]
        public IActionResult Service(LogEditViewModel model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                      
            var ticket = _tickets.Get(model.TicketId);
            //If this is the first log for a new ticket update the ticket status
            if (model.TicketStatus == TicketStatus.New)
            {
                //Whenever a ticket is modified make an entry in tickethistory before changing
                _ticketsHistory.AddTicketToHistory(ticket);
                model.TicketStatus = TicketStatus.InProgress;                
            }
            //create a new Log entry
            Log log = new Log
            {
                UserId = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                TicketId = model.TicketId,
                Logged = DateTime.Now,
                Notes = model.Notes,
                LogType = model.LogType,
                ContactMethod = model.ContactMethod
            };
            _logs.Add(log);
            _logs.Commit();
            //update the database with any changes that were made to the ticketstatus 
            ticket.TicketStatus = model.TicketStatus;
            _tickets.Commit();
            return RedirectToAction("Home", "Ticket");            
        }        
        //Log/Contact
        //Get Method to log contact with customer
        [HttpGet]
        public IActionResult Contact(int ticketId)
        {
            //get th ticket and the customer associated with that ticket
            var ticket = _tickets.Get(ticketId);
            var customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId);
            //create LogEditViewModel
            var model = new LogEditViewModel
            {
                TicketId = ticket.Id,
                TicketNumber = ticket.TicketNumber,
                Customer = customer,
                TicketStatus = ticket.TicketStatus,
                ContactMethod = ContactMethod.InPerson
            };
            //return View
            return View(model);
        }
        [HttpPost]
        //Post Method for log contact
        public IActionResult Contact(LogEditViewModel model)
        {
            //get the ticket and check for valid
            Ticket ticket = _tickets.Get(model.TicketId);
            if (!ModelState.IsValid || ticket == null)
            {
                return View(model);
            }
            if(ticket.TicketStatus != model.TicketStatus)
            {
                //Whenever a ticket is modified make an entry in tickethistory before changing
                _ticketsHistory.AddTicketToHistory(ticket);
            }
            //create new log
            Log log = new Log
            {
                UserId = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                TicketId = model.TicketId,
                Logged = DateTime.Now,
                Notes = model.Notes,
                LogType = model.LogType,
                ContactMethod = model.ContactMethod

            };
            //update database
            _logs.Add(log);
            _logs.Commit();
            //record time finished if ticket has been completed
            ticket.TicketStatus = model.TicketStatus;
            if (model.TicketStatus == TicketStatus.PendingPickup)
            {
                ticket.Finished = DateTime.Now;
            }
            else if (model.TicketStatus == TicketStatus.Done)
            {
                ticket.CheckedOut = DateTime.Now;
                ticket.CheckOutUserId = User.FindFirst(ClaimTypes.Name).Value.ToString();
            }
            //update tickets table
            _tickets.Commit();
            return RedirectToAction("Home", "Ticket");            
        }
    }
}