using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Services;
using Microsoft.AspNetCore.Mvc;

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
            
            return View();
        }
    }
}