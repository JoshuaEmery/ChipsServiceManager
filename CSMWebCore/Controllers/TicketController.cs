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

        public IActionResult Index()
        {
            
            return View();
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

    }
}