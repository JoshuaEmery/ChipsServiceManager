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
                ModelNumber = device.ModelNumber,
                OperatingSystem = device.OperatingSystem,
                Password = device.Password,
                Serviced = device.Serviced
            });
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            DeviceCreateViewModel model = new DeviceCreateViewModel();
            model.Customer = new List<SelectListItem>();
            foreach (var customer in _customers.GetAll())
            {
                model.Customer.Add(new SelectListItem(customer.FirstName + " " + customer.LastName, customer.Id.ToString()));
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(DeviceCreateViewModel model)
        {
            if(ModelState.IsValid)
            string text = model.Make;
            return View();
        }

    }
}