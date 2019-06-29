using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Entities;
using CSMWebCore.Services;
using CSMWebCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSMWebCore.Controllers
{
    public class DeviceController : Controller
    {
        private IDeviceData _devices;
        private ICustomerData _customers;
        public DeviceController(IDeviceData devices, ICustomerData customers)
        {
            _devices = devices;
            _customers = customers;
        }
        public IActionResult Index()
        {
            var model = _devices.GetAll().Select(device => new DeviceViewModel
            {
                Id = device.Id,
                Owner = _customers.Get(device.CustomerId),
                Make = device.Make,
                Model = device.Model,
                OperatingSystem = device.OperatingSystem,
                Password = device.Password,
                Serviced = device.Serviced
            });
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {            
            ViewBag.Customer = new List<SelectListItem>();
            foreach (var customer in _customers.GetAll())
            {
                ViewBag.Customer.Add(new SelectListItem(customer.FirstName + " " + customer.LastName,customer.Id.ToString()));
            }           
            return View();
        }
        [HttpPost]
        public IActionResult Create(Device model)
        {
            return View();
        }

    }
}