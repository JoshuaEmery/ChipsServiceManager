using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Services;
using CSMWebCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace CSMWebCore.Controllers
{
    //Conroller that generates data for updating the customer
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
        //Update/Index
        //Takes a string representation of a GUID and returns informaiton
        //aboout the ticket associated with that Guid
        public IActionResult Index(string id)
        {
            var guid = new Guid(id);
            var ticket = _tickets.GetById(_updates.GetTicketId(guid));
            var model = new UpdateViewModel
            {
                Ticket = ticket,
                Device = _devices.Get(ticket.DeviceId),
                Log = _logs.GetLastByTicketId(ticket.Id),
            };
            return View(model);            
        }

    }
}