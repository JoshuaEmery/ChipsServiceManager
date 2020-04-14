using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Services;
using CSMWebCore.Shared;
using CSMWebCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace CSMWebCore.Controllers
{
    //Conroller that generates data for updating the customer
    public class UpdateController : Controller
    {
        private ChipsDbContext context;
        //private IDeviceRepository _devices;
        //private ICustomerRepository _customers;
        //private ITicketRepository _tickets;
        //private ILogRepository _logs;
        private IUpdateData _updates;

        public UpdateController(ChipsDbContext context, IUpdateData updates)
        {
            this.context = context;
            //_devices = devices;
            //_customers = customers;
            //_tickets = tickets;
            //_logs = logs;
            _updates = updates;
        }
        //Update/Index
        //Takes a string representation of a GUID and returns informaiton
        //aboout the ticket associated with that Guid
        public IActionResult Index(string id)
        {
            var guid = new Guid(id);
            var ticket = context.Tickets.Find(_updates.GetTicketId(guid));
            var model = new ConfirmationViewModel
            {
                Ticket = ticket,
                Device = context.Devices.Find(ticket.Device.Id),
                Log = context.Logs.GetLatestLogByTicketId(ticket.Id),
            };
            return View(model);            
        }

    }
}