using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CSMWebCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSMWebCore.Controllers
{
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
            //var userName = User.FindFirst(ClaimTypes.Name).Value;

            return View();
        }
        public IActionResult CreateNew()
        {
            return View();
        }
        public IActionResult CreateExisting()
        {
            return View();
        }
    }
}