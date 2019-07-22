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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSMWebCore.Controllers
{
    [Authorize]
    public class DeviceController : Controller
    {
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;
        public DeviceController(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
        }
        public IActionResult Index()
        {
            var model = _devices.GetAll().Select(device => new DeviceViewModel
            {
                Id = device.Id,
                Owner = _customers.Get(device.CustomerId),
                Make = device.Make,
                ModelNumber = device.ModelNumber,
                OperatingSystem = device.OperatingSystem,
                Password = device.Password,
                Serviced = device.Serviced
            });
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int deviceId)
        {
            var device = _devices.Get(deviceId);
            if (device == null)
            {
                return View();
            }
            DeviceEditViewModel model = new DeviceEditViewModel
            {
                Id = device.Id,
                CustomerId = device.CustomerId,
                Make = device.Make,
                ModelNumber = device.ModelNumber,
                OperatingSystem = device.OperatingSystem,
                Password = device.Password,
                Serviced = device.Serviced
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(DeviceEditViewModel model)
        {
            var device = _devices.Get(model.Id);
            if (device == null || !ModelState.IsValid)
            {
                return View();
            }
            device.CustomerId = model.CustomerId;
            device.Make = model.Make;
            device.ModelNumber = model.ModelNumber;
            device.OperatingSystem = model.OperatingSystem;
            device.Password = model.Password;
            device.Serviced = model.Serviced;
            _devices.Commit();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create()
        {
            DeviceEditViewModel model = new DeviceEditViewModel();
            model.Customer = new List<SelectListItem>();
            foreach (var customer in _customers.GetAll())
            {
                model.Customer.Add(new SelectListItem(customer.FirstName + " " + customer.LastName, customer.Id.ToString()));
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(DeviceEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Device device = new Device
                {
                    CustomerId = model.CustomerId,
                    Make = model.Make,
                    ModelNumber = model.ModelNumber,
                    OperatingSystem = model.OperatingSystem,
                    Password = model.Password,
                    Serviced = model.Serviced
                };
                _devices.Add(device);
                _devices.Commit();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Details(int deviceId)
        {
            var device = _devices.Get(deviceId);
            if (device == null)
            {
                return View();
            }
            var model = new DeviceEditViewModel
            {
                Id = device.Id,
                Owner = _customers.Get(device.CustomerId),
                Make = device.Make,
                ModelNumber = device.ModelNumber,
                OperatingSystem = device.OperatingSystem,
                Password = device.Password,
                Serviced = device.Serviced
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult CreateByCustId(int customerId)
        {
            DeviceEditViewModel model = new DeviceEditViewModel();
            model.Ticket = new Ticket();
            model.CustomerId = customerId;
            model.Owner = _customers.Get(customerId);
            model.Ticket.TicketNumber = _tickets.CurrentTicketNumber() + 1;
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateByCustId(DeviceEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Owner = _customers.Get(model.CustomerId);
                return View(model);

            }
            Device device = new Device
            {
                CustomerId = model.CustomerId,
                Make = model.Make,
                ModelNumber = model.ModelNumber,
                OperatingSystem = model.OperatingSystem,
                Password = model.Password,
                Serviced = model.Serviced
            };
            _devices.Add(device);
            _devices.Commit();
            Ticket ticket = new Ticket
            {
                DeviceId = device.Id,
                CheckInUserId = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                CheckedIn = DateTime.Now,
                NeedsBackup = model.Ticket.NeedsBackup,
                TicketStatus = TicketStatus.New,
                TicketNumber = model.Ticket.TicketNumber
            };
            _tickets.Add(ticket);
            _tickets.Commit();
            Log log = new Log
            {
                UserId = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                TicketId = ticket.Id,
                Logged = DateTime.Now,
                Notes = model.Log.Notes,
                LogType = LogType.CheckIn,
                ContactMethod = ContactMethod.InPerson
            };
            _logs.Add(log);
            _logs.Commit();
            return RedirectToAction("Home", "Ticket", null);
        }
        [HttpGet] public IActionResult DevicesByCustId(int customerId)
        {
            var cust = _customers.Get(customerId);
            if(cust == null)
            {
                return View();
            }
            ViewBag.Customer = cust;
            var model = _devices.GetAllByCustId(customerId).Select(device => new DeviceViewModel
            {
                Id = device.Id,
                CustomerId = device.CustomerId,
                Make = device.Make,
                ModelNumber = device.ModelNumber,
                OperatingSystem = device.OperatingSystem,
                Password = device.Password,
                Serviced = device.Serviced
                
            });
            return View(model);


        }

    }
}