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
        public IActionResult Edit(int id)
        {
            var device = _devices.Get(id);
            if (device == null)
            {
                return View();
            }
            DeviceEditViewModel model = new DeviceEditViewModel
            {
                Make = device.Make,
                ModelNumber = device.ModelNumber,
                OperatingSystem = device.OperatingSystem,
                Password = device.Password,
                Serviced = device.Serviced
            };
            model.Customer = new List<SelectListItem>();
            foreach (var customer in _customers.GetAll())
            {
                model.Customer.Add(new SelectListItem(customer.FirstName + " " + customer.LastName, customer.Id.ToString(), customer.Id == device.CustomerId ? true : false));
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(DeviceEditViewModel model)
        {
            var device = _devices.Get(model.Id);
            if (device == null || !ModelState.IsValid )
            {
                return View();
            }
            device.CustomerId = int.Parse(model.CustomerId);
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
                    CustomerId = int.Parse(model.CustomerId),
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
        public IActionResult Details(int id)
        {
            var device = _devices.Get(id);
            if(device == null)
            {
                return View();
            }
            var model = new DeviceViewModel
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

    }
}