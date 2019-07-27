using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSMWebCore.Controllers
{
    public class UpdateController : Controller
    {
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;
        private IUpdateData _updates;

        public UpdateController(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs, IUpdateData updates)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
            _updates = updates;
        }
        public IActionResult Index(string id)
        {
            var guid = new Guid(id);
            var ticket = _tickets.Get(_updates.GetTicketId(guid));
            return View(ticket);
        }
    }
}