using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Entities;
using CSMWebCore.Services;
using CSMWebCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSMWebCore.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private ICustomerData _customers;
        private ITicketData _tickets;
        private IDeviceData _devices;
        public CustomerController(ICustomerData customers, ITicketData tickets, IDeviceData devices)
        {
            _customers = customers;
            _tickets = tickets;
            _devices = devices;
        }

        public IActionResult Index()
        {
            var model = _customers.GetAll().Select(cust =>
            new CustomerViewModel
            {
                Id = cust.Id,
                FirstName = cust.FirstName,
                LastName = cust.LastName,
                Email = cust.Email,
                StudentId = cust.StudentId,
                Phone = cust.Phone,
                ContactPref = cust.ContactPref.ToString()
            });
            return View(model);
        }
        public IActionResult Details(int id)
        {
            var cust = _customers.Get(id);
            if(cust == null)
            {
                RedirectToAction("Index");
            }
            return View(new CustomerViewModel
            {
                Id = cust.Id,
                FirstName = cust.FirstName,
                LastName = cust.LastName,
                Email = cust.Email,
                StudentId = cust.StudentId,
                Phone = cust.Phone,
                ContactPref = cust.ContactPref.ToString()
            });
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cust = _customers.Get(id);
            if(cust == null)
            {
                return RedirectToAction("Index");
            }
            CustomerEditViewModel model = new CustomerEditViewModel
            {
                Id = cust.Id,

            };
            return View(cust);
        }
        [HttpPost]
        public IActionResult Edit(CustomerEditViewModel model)
        {
            var cust = _customers.Get(model.Id);
            if(cust == null || !ModelState.IsValid)
            {
                return View(model);
            }
            cust.FirstName = model.FirstName;
            cust.LastName = model.LastName;
            cust.StudentId = model.StudentId;
            cust.Email = model.Email;
            cust.Phone = model.Phone;
            cust.ContactPref = model.ContactPref;
            _customers.Commit();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost] 
        public IActionResult Create(CustomerEditViewModel model)
        {
            Random rand = new Random();
            if (ModelState.IsValid)
            {
                Customer cust = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    StudentId = model.StudentId,
                    ContactPref = model.ContactPref
                };
                _customers.Add(cust);
                _customers.Commit();
                int newId = cust.Id;
                return RedirectToAction("Details", new { id = newId });
            }
            return View();
        }
        
        public IActionResult Search(string searchValue)
        {
            var model = _customers.Search(searchValue).Select(cust => new CustomerViewModel {
                Id = cust.Id,
                FirstName = cust.FirstName,
                LastName = cust.LastName,
                Email = cust.Email,
                StudentId = cust.StudentId,
                Phone = cust.Phone,
                ContactPref = cust.ContactPref.ToString()
            });
            ViewBag.SearchValue = searchValue;
            return View(model);
        }
        public IActionResult Active()
        {
            //List<Customer> activeCustomers = new List<Customer>();
            //List<Device> activeDevices = new List<Device>();
            //var activeTickets = _tickets.GetAllActiveTickets();           
            //foreach (var ticket in activeTickets)
            //{
            //    activeDevices.Add(_devices.Get(ticket.DeviceId));
            //}
            //foreach (var device in activeDevices)
            //{
            //    activeCustomers.Add(_customers.Get(device.CustomerId));
            //}
            //return View("Search", activeCustomers);
            var model = _tickets.GetAllActiveTickets().Select(ticket => new CustomerActiveViewModel
            {
                Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId)
                
            });
            return View(model);



        }
    }
}