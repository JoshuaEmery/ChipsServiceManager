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
    [Authorize]
    public class TicketController : Controller
    {
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;

        public TicketController(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
        }
        //Begin Test Actions
        public IActionResult Index()
        {
            var model = _tickets.GetAll().Select(cust => new TicketViewModel
            {
                Id = cust.Id,
                DeviceId = cust.DeviceId,
                CheckedIn = cust.CheckedIn.ToShortDateString(),
                CheckedOut = cust.CheckedOut.ToShortDateString(),
                Finished = cust.Finished.ToShortDateString(),
                CheckInUserId = cust.CheckInUserId,
                CheckOutUserId = cust.CheckOutUserId,
                NeedsBackup = cust.NeedsBackup,
                TicketStatus = cust.TicketStatus.ToString()
            });
            return View(model);
        }
        [HttpGet]
        public IActionResult Create(int deviceId)
        {
            var device = _devices.Get(deviceId);
            var cust = _customers.Get(device.CustomerId);            
            if(cust == null || device == null)
            {
                return NotFound();
            }
            TicketEditViewModel model = new TicketEditViewModel
            {
                Customer = cust,
                Device = device
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(TicketEditViewModel model)
        {
            if(model == null || !ModelState.IsValid)
            {
                return View();
            }
            _tickets.Add(
                new Ticket { DeviceId = model.DeviceId,
                    CheckInUserId = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                    CheckedIn = DateTime.Now,
                    NeedsBackup = model.NeedsBackup,
                    TicketStatus = TicketStatus.New                               
                });
            _tickets.Commit();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _tickets.Get(id);
            if(model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(TicketEditViewModel model)
        {
            var ticket = _tickets.Get(model.Id);
            if (ticket == null || !ModelState.IsValid)
            {
                return View(model);
            }
            ticket.DeviceId = model.DeviceId;
            ticket.CheckedIn = model.CheckedIn;
            ticket.CheckedOut = model.CheckedOut;
            ticket.Finished = model.Finished;
            ticket.CheckInUserId = model.CheckInUserId;
            ticket.CheckOutUserId = model.CheckOutUserId;
            ticket.NeedsBackup = model.NeedsBackup;
            ticket.TicketStatus = model.TicketStatus;
            _tickets.Commit();
            return RedirectToAction("Home");

        }
        //End Test Actions
        //Begin Launch Actions
        [HttpGet]
        public IActionResult Home()
        {
            var model = _tickets.GetAll().Select(ticket => new TicketHomeViewModel
            {
                Ticket = ticket,
                Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                Log = _logs.GetLastByTicketId(ticket.Id),
                Logs = _logs.GetLogsByTicketId(ticket.Id)     
                
            });
            
            return View(model);
        }
        [HttpPost]
        public IActionResult Home(TicketHomeViewModel result)
        {
            var model = _tickets.GetByStatus(result.TicketStatus).Select(ticket => new TicketHomeViewModel
            {                
                Ticket = ticket,
                Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                Log = _logs.GetLastByTicketId(ticket.Id),
                Logs = _logs.GetLogsByTicketId(ticket.Id)

            });
            return View(model);
        }




    }
}