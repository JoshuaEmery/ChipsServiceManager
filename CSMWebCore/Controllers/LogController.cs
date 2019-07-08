using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSMWebCore.Controllers
{
    [Authorize]
    public class LogController : Controller
    {
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;

        public LogController(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs)
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
        public IActionResult Create()
        {
            return View();
        }
    }
}