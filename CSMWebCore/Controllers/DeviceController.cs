﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;
using CSMWebCore.Enums;
using CSMWebCore.Models;
using CSMWebCore.Services;
using CSMWebCore.Shared;
using CSMWebCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QRCoder;

namespace CSMWebCore.Controllers
{
    //Check for Login
    [Authorize]
    public class DeviceController : Controller
    {
        private ChipsDbContext context;
        private IUpdateData _updates;
        private ITicketCreator _ticketCreator;
        //constructor
        public DeviceController(ChipsDbContext context, IUpdateData updates, ITicketCreator ticketCreator)
        {
            this.context = context;
            _updates = updates;
            _ticketCreator = ticketCreator;
        }
        //Device/Index
        // returns view for list of devices assigned to *active* tickets
        public IActionResult Index()
        {
            // create list
            List<NewDeviceViewModel> model = new List<NewDeviceViewModel>();
            //Get all active tickets
            var activetickets = context.Tickets.GetOpenTickets();            
            // sequence thru active tickets, add viewmodel for each device to list     
            foreach (var ticket in activetickets)
            {
                var device = context.Devices.Find(ticket.DeviceId);
                model.Add(new NewDeviceViewModel
                {
                    Id = device.Id,
                    CustomerFirstName = context.Customers.Find(device.CustomerId).FirstName,
                    CustomerLastName = context.Customers.Find(device.CustomerId).LastName,
                    TicketNumber = ticket.TicketNumber,
                    TicketStatus = ticket.Status,
                    Make = device.Make,
                    ModelNumber = device.ModelNumber,
                    OperatingSystem = device.OperatingSystem,
                    Password = device.Password,
                    Serviced = device.Serviced
                });
            }
            // pass viewmodel to view, return
            return View(model);
        }
        //Device/Edit
        //Edit method for devices
        [HttpGet]
        public IActionResult Edit(int deviceId)
        {
            //Check to see if device exists
            var device = context.Devices.Find(deviceId);
            if (device == null)
            {
                return View();
            }
            //create new DeviceEditViewModel
            NewDeviceEditViewModel model = new NewDeviceEditViewModel
            {
                Id = device.Id,
                CustomerId = device.CustomerId,
                Make = device.Make,
                ModelNumber = device.ModelNumber,
                OperatingSystem = device.OperatingSystem,
                Password = device.Password,
                Serviced = device.Serviced
            };
            //return model
            return View(model);
        }
        //Post for Edit
        [HttpPost]
        public IActionResult Edit(NewDeviceEditViewModel model)
        {
            //Check to see that the device exists
            var device = context.Devices.Find(model.Id);
            if (device == null || !ModelState.IsValid)
            {
                return View();
            }
            //update device and save changes
            device.CustomerId = model.CustomerId;
            device.Make = model.Make;
            device.ModelNumber = model.ModelNumber;
            device.OperatingSystem = model.OperatingSystem;
            device.Password = model.Password;
            device.Serviced = model.Serviced;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        //Device/Detials
        //Method that Shows the details of a device by ID
        public IActionResult Details(int deviceId)
        {
            //Check if device exists
            var device = context.Devices.Find(deviceId);
            if (device == null)
            {
                return View();
            }
            //create and return ViewModel
            var model = new NewDeviceDetailsViewModel
            {
                Id = device.Id,
                CustomerId = context.Customers.Find(device.CustomerId).Id,
                Make = device.Make,
                ModelNumber = device.ModelNumber,
                OperatingSystem = device.OperatingSystem,
                Password = device.Password,
                Serviced = device.Serviced
            };
            return View(model);
        }
        //Device/CreateByCustId
        //Create a new Device, in order to create a new device a customer must
        //first be selected
        [HttpGet]
        public IActionResult CreateByCustId(int id)
        {
            //The DeviceEditViewModel stores a Ticket, a Customer and TicketID and CustomerID
            //On get Customer is populated, on Post Ticket is populated, the customer will
            //have to be retrieved again if needed on post.

            Customer cust = context.Customers.Find(id);

            var model = new NewDeviceCreateViewModel 
            { 
                TicketNumber = context.Tickets.GetLatestTicketNum() + 1,
                CustomerId = id,
                CustomerFirstName = cust.FirstName,
                CustomerLastName = cust.LastName
            };
            return View(model);
        }
        //Post for CreateuByCustId
        //Oh this post 4 object need to be created Device, Ticket, Log, and Update
        //with each subsequent object using the ID of the previous one as a foreign Key.
        //The Update table is essentially a reference table with a Guid and a TicketNumber.
        //The Guid is stored in a QRCode.  Another thing to keep in mind while looking at this method
        //is the difference between TicketNumber and TicketId.  These are seperate depsite both
        //being unique to each ticket.  There is no constraint on the TicketNumber other than
        //what is enforced here.
        [HttpPost]
        public IActionResult CreateByCustId(NewDeviceCreateViewModel model)
        {
            
            //Check if Model State is Valid
            if (!ModelState.IsValid)
            {             
                return View(model);
            }
            //Create a new device using the Customer ID and form data
            Device device = new Device
            {
                CustomerId = model.CustomerId,
                Make = model.Make,
                ModelNumber = model.ModelNumber,
                OperatingSystem = model.OperatingSystem,
                Password = model.Password,
                Serviced = model.Serviced
            };
            //Save the new device
            context.Add(device);
            context.SaveChanges();
            TicketConfirmationModel tcModel = _ticketCreator.CreateTicket(new TicketCreatorInfo
            {
                DeviceId = device.Id,
                CustomerId = model.CustomerId,
                NeedsBackup = model.TicketNeedsBackup,
                Notes = model.LogNotes,
                UserName = User.FindFirst(ClaimTypes.Name).Value.ToString()
            });
            return RedirectToAction("Confirmation", "Ticket", new { ticketId = tcModel.ticketId,
            deviceId = tcModel.deviceId,
            customerId = tcModel.customerId,
            updateId = tcModel.updateId});
        }
        //Device/DevicesByCustId
        //Method that gets all devices owned by a given customer
        [HttpGet]
        public IActionResult DevicesByCustId(int id)
        {
            //check to see if customer exists
            var cust = context.Customers.Find(id);
            if (cust == null)
            {
                return View();
            }
            ViewBag.CustomerFirstName = cust.FirstName;
            ViewBag.CustomerLastName = cust.LastName;
            //create a IEnumerable of DeviceViewModel by customer ID
            var model = context.Devices.GetDevicesByCustId(id).Select(device => new NewDevicesByCustIdViewModel
            {
                Id = device.Id,
                Make = device.Make,
                ModelNumber = device.ModelNumber,
                OperatingSystem = device.OperatingSystem
            });
            return View(model);
        }
        //Device/Search
        public IActionResult Search(string searchValue)
        {
            //create an IEnumerable of DeviceViewModel from using the searchValue, 
            //see SQLDevice for Search method
            var model = context.Devices.Search(searchValue).Select(device => new NewDeviceViewModel
            {
                Id = device.Id,
                CustomerFirstName = device.Customer.FirstName,
                CustomerLastName = device.Customer.LastName,
                Make = device.Make,
                ModelNumber = device.ModelNumber,
                OperatingSystem = device.OperatingSystem,
                Password = device.Password,
                Serviced = device.Serviced,
                TicketNumber = context.Tickets.GetLatestTicketForDevice(device.Id).TicketNumber,
                TicketStatus = context.Tickets.GetLatestTicketForDevice(device.Id).Status

            });
            return View("Index", model);
        }
    }
}