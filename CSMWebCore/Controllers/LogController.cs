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
        private IDeviceRepository _devices;
        private ICustomerRepository _customers;
        private ITicketRepository _tickets;
        private ILogRepository _logs;
        private ITicketsHistoryData _ticketsHistory;

        public LogController(IDeviceRepository devices, ICustomerRepository customers, ITicketRepository tickets, ILogRepository logs, ITicketsHistoryData ticketsHistory)
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
        //    var model = _logs.Get().Select(log => new LogViewModel
        //    {
        //        Id = log.Id,
        //        UserCreated = log.UserCreated,
        //        TicketId = log.TicketId,
        //        DateCreated = log.DateCreated.ToShortDateString(),
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
        //            UserCreated = User.FindFirst(ClaimTypes.Name).Value.ToString(),
        //            TicketId = model.TicketId,
        //            DateCreated = DateTime.Now,
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
        //    log.UserCreated = model.UserCreated;
        //    log.TicketId = model.TicketId;
        //    log.DateCreated = model.DateCreated;
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
            var ticket = _tickets.GetById(ticketId);            
            //create LogEditViewModel
            var model = new NewLogServiceViewModel
            {
                TicketId = ticket.Id,
                TicketNumber = ticket.TicketNumber,
                TicketStatus = _logs.GetLastByTicketId(ticketId).TicketStatus
            };
            return View(model);
        }
        //Post for /Log/Service
        [HttpPost]
        public IActionResult Service(NewLogServiceViewModel model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.TicketStatus == TicketStatus.New)
                model.TicketStatus = TicketStatus.InProgress;                      
            var ticket = _tickets.GetById(model.TicketId);
            Log log = new Log
            {
                UserCreated = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                TicketId = model.TicketId,
                DateCreated = DateTime.Now,
                Notes = model.LogNotes,                
                ContactMethod = ContactMethod.NoContact,
                EventId = (int)model.EventName,
                TicketStatus = model.TicketStatus
            };
            _logs.Insert(log);
            _logs.Commit();
            //update the database with any changes that were made to the ticketstatus 
            _tickets.Commit();
            return RedirectToAction("Index", "Ticket");            
        }        
        //Log/Contact
        //Get Method to log contact with customer
        [HttpGet]
        public IActionResult Contact(int ticketId)
        {
            //get th ticket and the customer associated with that ticket
            var ticket = _tickets.GetById(ticketId);
            var customer = _customers.GetById(_devices.GetById(ticket.DeviceId).CustomerId);
            //create LogEditViewModel
            var model = new LogEditViewModel
            {
                TicketId = ticket.Id,
                TicketNumber = ticket.TicketNumber,
                Customer = customer,
                TicketStatus = ticket.Status,
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
            Ticket ticket = _tickets.GetById(model.TicketId);
            if (!ModelState.IsValid || ticket == null)
            {
                return View(model);
            }
            if(ticket.Status != model.TicketStatus)
            {
                //Whenever a ticket is modified make an entry in tickethistory before changing
                _ticketsHistory.AddTicketToHistory(ticket);
            }
            //create new log
            Log log = new Log
            {
                UserCreated = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                EventId = (int)EventCategory.Contact,
                TicketId = model.TicketId,
                DateCreated = DateTime.Now,
                Notes = model.Notes,
                LogType = model.LogType,
                ContactMethod = model.ContactMethod

            };
            //update database
            _logs.Insert(log);
            _logs.Commit();
            //record time finished if ticket has been completed
            ticket.Status = model.TicketStatus;
            if (model.TicketStatus == TicketStatus.PendingPickup)
            {
                ticket.FinishDate = DateTime.Now;
            }
            else if (model.TicketStatus == TicketStatus.Closed)
            {
                ticket.CheckOutDate = DateTime.Now;
                ticket.CheckOutUserId = User.FindFirst(ClaimTypes.Name).Value.ToString();
            }
            //update tickets table
            _tickets.Commit();
            return RedirectToAction("Index", "Ticket");            
        }
    }
}