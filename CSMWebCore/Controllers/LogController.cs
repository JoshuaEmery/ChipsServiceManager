using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;
using CSMWebCore.Enums;
using CSMWebCore.Services;
using CSMWebCore.Shared;
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
        private ChipsDbContext context;

        public LogController(ChipsDbContext context)
        {
            this.context = context;
        }
        //Test Actions
        //public IActionResult Index()
        //{
        //    var model = context.Logs.Get().Select(log => new LogViewModel
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
        //        context.Logs.Add(log);
        //        context.Logs.Commit();
        //        return RedirectToAction("Index");
        //    }
        //    return View();

        //}
        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    var model = context.Logs.Get(id);
        //    if (model == null)
        //    {
        //        return View();
        //    }
        //    return View(model);
        //}
        //[HttpPost]
        //public IActionResult Edit(LogEditViewModel model)
        //{
        //    var log = context.Logs.Get(model.Id);
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
        //    context.Logs.Commit();
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
            var ticket = context.Tickets.Find(ticketId);
            IEnumerable<Event> events = context.Events.Where(e => e.Category == EventCategory.HWService || e.Category == EventCategory.SWService).ToList();
            //create LogEditViewModel
            var model = new NewLogServiceViewModel
            {
                TicketId = ticket.Id,
                TicketNumber = ticket.TicketNumber,
                TicketStatus = context.Logs.GetLatestLogByTicketId(ticketId).TicketStatus,
                Events = SelectListHelper.ToSelectListItems(events, -1) // nothing selected
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
            var ticket = context.Tickets.Find(model.TicketId);
            Log log = new Log
            {
                UserCreated = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                TicketId = model.TicketId,
                DateCreated = DateTime.Now,
                Notes = model.LogNotes,                
                ContactMethod = ContactMethod.NoContact,
                EventId = model.SelectedEventId,
                TicketStatus = model.TicketStatus
            };
            context.Add(log);
            //update the database with any changes that were made to the ticketstatus 
            context.SaveChanges();
            return RedirectToAction("Index", "Ticket");            
        }        
        //Log/Contact
        //Get Method to log contact with customer
        [HttpGet]
        public IActionResult Contact(int ticketId)
        {
            //get th ticket and the customer associated with that ticket
            var ticket = context.Tickets.Find(ticketId);
            var customer = context.Customers.Find(context.Devices.Find(ticket.DeviceId).CustomerId);
            IEnumerable<Event> events = context.Events.Where(e => e.Category == EventCategory.Contact).ToList();
            //create LogEditViewModel
            var model = new NewLogContactViewModel
            {
                TicketId = ticket.Id,
                TicketNumber = ticket.TicketNumber,
                CustomerFirstName = customer.FirstName,
                CustomerLastName = customer.LastName,
                CustomerEmail = customer.Email,
                CustomerPhone = customer.Phone,
                TicketStatus = context.Logs.GetLatestLogByTicketId(ticketId).TicketStatus,
                Events = SelectListHelper.ToSelectListItems(events, -1) // nothing selected
            };
            //return View
            return View(model);
        }
        [HttpPost]
        //Post Method for log contact
        public IActionResult Contact(NewLogContactViewModel model)
        {
            //get the ticket and check for valid
            Ticket ticket = context.Tickets.Find(model.TicketId);
            if (!ModelState.IsValid || ticket == null)
            {
                return View(model);
            }
            //create new log
            Log log = new Log
            {
                UserCreated = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                EventId = model.SelectedEventId,
                TicketId = model.TicketId,
                DateCreated = DateTime.Now,
                Notes = model.LogNotes,
                TicketStatus = model.TicketStatus
            };
            //update database
            context.Add(log);
            context.SaveChanges();
            ////record time finished if ticket has been completed
            //ticket.Status = model.TicketStatus;
            //if (model.TicketStatus == TicketStatus.PendingPickup)
            //{
            //    ticket.FinishDate = DateTime.Now;
            //}
            //else if (model.TicketStatus == TicketStatus.Closed)
            //{
            //    ticket.CheckOutDate = DateTime.Now;
            //    ticket.CheckOutUserId = User.FindFirst(ClaimTypes.Name).Value.ToString();
            //}
            ////update tickets table
            //context.Tickets.Commit();
            return RedirectToAction("Index", "Ticket");            
        }
    }
}