using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CSMWebCore.Entities;
using CSMWebCore.Models;
using CSMWebCore.Services;
using CSMWebCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace CSMWebCore.Controllers
{
    //Check for Authorization
    [Authorize]
    public class TicketController : Controller
    {
        //Ticket Controller needs access to all 5 tables
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;
        private IUpdateData _updates;
        //constructor
        public TicketController(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs, IUpdateData updates)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
            _updates = updates;
        }
        //Ticket/Index
        //This method was used early in testing, I dont think the application still uses it for anything
        public IActionResult Index()
        {
            var model = _tickets.GetAll().Select(cust => new TicketViewModel
            {
                Id = cust.Id,
                DeviceId = cust.DeviceId,
                CheckedIn = cust.CheckedIn.ToShortDateString(),
                CheckedOut = cust.CheckedOut.ToShortDateString(),
                Finished = cust.Finished.ToShortDateString(),
                CheckInUserId = cust.CheckInUserId,
                CheckOutUserId = cust.CheckOutUserId,
                NeedsBackup = cust.NeedsBackup,
                TicketStatus = cust.TicketStatus.ToString()
            });
            return View(model);
        }
        //Ticket/CreateByExistingDeviceId
        //Method for creating a ticket when a device has already been created.  That also
        //means that a customer has already been created.
        [HttpGet]
        public IActionResult CreateByExistingDeviceId(int deviceId)
        {
            //get the device and check it
            var device = _devices.Get(deviceId);
            if (device == null)
            {
                return View();
            }
            //If it exists create ViewModel and send it to the View
            DeviceEditViewModel model = new DeviceEditViewModel
            {
                Id = device.Id,
                Make = device.Make,
                ModelNumber = device.ModelNumber,
                OperatingSystem = device.OperatingSystem,
                Password = device.Password,
                Serviced = device.Serviced,
                Ticket = new Ticket(),
                Owner = _customers.Get(device.CustomerId),
                CustomerId = device.CustomerId

            };
            //Get the current Ticket Number
            model.Ticket.TicketNumber = _tickets.CurrentTicketNumber() + 1;
            return View(model);
        }
        //Post for CreateByExistingDeviceId
        [HttpPost]
        public IActionResult CreateByExistingDeviceId(DeviceEditViewModel model)
        {
            //Get Ticket Number again and check ModelState
            model.Ticket.TicketNumber = _tickets.CurrentTicketNumber() + 1;
            if (!ModelState.IsValid)
            {
                model.Owner = _customers.Get(model.CustomerId);
                return View(model);

            }
            //get the device and create a new ticket from the device
            var device = _devices.Get(model.Id);
            Ticket ticket = new Ticket
            {
                DeviceId = device.Id,
                CheckInUserId = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                CheckedIn = DateTime.Now,
                NeedsBackup = model.Ticket.NeedsBackup,
                TicketStatus = TicketStatus.New,
                TicketNumber = model.Ticket.TicketNumber
            };
            //save changes
            _tickets.Add(ticket);
            _tickets.Commit();
            //create a new log
            Log log = new Log
            {
                UserId = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                TicketId = ticket.Id,
                Logged = DateTime.Now,
                Notes = model.Log.Notes,
                LogType = LogType.CheckIn,
                ContactMethod = ContactMethod.InPerson
            };
            //save changes
            _logs.Add(log);
            _logs.Commit();
            //create a new Update
            Update update = new Update
            {
                Id = new Guid(),
                TicketId = ticket.Id
            };
            //save changes
            _updates.Add(update);
            _updates.Commit();
            //route to confirmation Page
            return RedirectToAction("Confirmation", new
            {
                ticketId = ticket.Id,
                deviceId = device.Id,
                customerId = model.CustomerId,
                updateId = update.Id
            });
        }
        [HttpGet]
        //Ticket/Edit
        //Method that allows editing of a Ticket
        public IActionResult Edit(int id)
        {
            var model = _tickets.Get(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
        //Post for Edit
        [HttpPost]
        public IActionResult Edit(TicketEditViewModel model)
        {
            //get the ticket to edit
            var ticket = _tickets.Get(model.Id);
            if (ticket == null || !ModelState.IsValid)
            {
                return View(model);
            }
            //edit the ticket
            ticket.DeviceId = model.DeviceId;
            ticket.CheckedIn = model.CheckedIn;
            ticket.CheckedOut = model.CheckedOut;
            ticket.Finished = model.Finished;
            ticket.CheckInUserId = model.CheckInUserId;
            ticket.CheckOutUserId = model.CheckOutUserId;
            ticket.NeedsBackup = model.NeedsBackup;
            ticket.TicketStatus = model.TicketStatus;
            //save changes
            _tickets.Commit();
            return RedirectToAction("Home");

        }
        //Ticket/Home
        //This is the Method that is called to display the Primary Ticket Page, this really shoud
        //be Ticket/Index
        public IActionResult Home()
        {
            //create an IEnumerable of TicketHomeViewModel from the active tickets IENumerable
            var model = _tickets.GetAllActiveTickets().Select(ticket => new TicketHomeViewModel
            {
                Ticket = ticket,
                Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                Log = _logs.GetLastByTicketId(ticket.Id),
                ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

            });
            //return
            return View(model);
        }
        //Ticket/FilterByStatus
        //Method that is called by the status drop down menu on click located in Ticket/Home.cshtml
        //The only property of the ViewModel that will be populated is the TicketStatus Property when this
        //method is called.
        [HttpPost]
        public IActionResult FilterByStatus(TicketHomeViewModel result)
        {
            //If they chose all show the default Ticket/Home View
            if (result.Status == "All")
            {
                return RedirectToAction("Home");
            }
            //Create IEnumerable of Tickets that match the Ticketstatus given
            var model = _tickets.GetByStatus(result.TicketStatus).Select(ticket => new TicketHomeViewModel
            {
                Ticket = ticket,
                Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                Log = _logs.GetLastByTicketId(ticket.Id),
                ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)
            });
            //return
            return View("Home", model);
        }
        //Ticket/FilterByDate
        //Method that is called by the date drop down menu on click located in Ticket/Home.cshtml
        //The only property of the ViewModel that will be populated is the DateFilter Property when this
        //method is called.
        [HttpPost]
        public IActionResult FilterByDate(TicketHomeViewModel result)
        {
            //If all return default Ticket/Home View
            if (result.Status == "All")
            {
                return RedirectToAction("Home");
            }
            //Check which filter was used and return corresponsding IENumerable of TicketHomeViewModel
            else if (result.DateFilter == DateFilter.Oldest)
            {
                var model = _tickets.GetAll().OrderBy(x => x.CheckedIn).Select(ticket => new TicketHomeViewModel
                {
                    Ticket = ticket,
                    Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                    Log = _logs.GetLastByTicketId(ticket.Id),
                    ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                    ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

                });

                return View("Home", model);
            }
            else if (result.DateFilter == DateFilter.Newest)
            {
                var model = _tickets.GetAll().OrderByDescending(x => x.CheckedIn).Select(ticket => new TicketHomeViewModel
                {
                    Ticket = ticket,
                    Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                    Log = _logs.GetLastByTicketId(ticket.Id),
                    ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                    ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

                });

                return View("Home", model);
            }
            else if (result.DateFilter == DateFilter.Idle)
            {
                var model = _tickets.GetAll().OrderBy(x => x.CheckedIn).Select(ticket => new TicketHomeViewModel
                {
                    Ticket = ticket,
                    Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                    Log = _logs.GetLastByTicketId(ticket.Id),
                    ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                    ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id),
                    DaysIdle = DateTime.Now - _logs.GetLastByTicketId(ticket.Id).Logged
                });
                var sorted = model.OrderByDescending(x => x.DaysIdle).ToList();

                return View("Home", sorted);
            }
            //this should never run, but if it does return default view
            return View("Home");
        }
        //Ticket/TicketsByDeviceId
        //Method that gets all tickets for a given device
        public IActionResult TicketsByDeviceId(int deviceId)
        {
            //Create an IEnumerable of TicketHomeViewModel that match the given deviceID
            var model = _tickets.GetAllByDevice(deviceId).Select(ticket => new TicketHomeViewModel
            {
                Ticket = ticket,
                Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                Log = _logs.GetLastByTicketId(ticket.Id),
                ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

            });
            //Check if model is null
            if (model != null)
            {
                return View("Home",model);
            }
            return View();
        }
        //Ticket/Search
        //Method that performs a basic search on the Tickets.  This is actually somewhat useless 
        //compared to the device or customer search.  This only searches the Ticket Table and most
        //of the Data stored in the ticket table is not easily searched, could be used to search
        //by ticket number, or checkin technician.  Other than that it is easier to find a specific
        //ticket by first finding the customer.
        public IActionResult Search(string searchValue)
        {
            //Create an IEnumerable of TicketHomeViewModel that match the searchvalue.  Check SQLTicket for
            //search method
            var model = _tickets.Search(searchValue).Select(ticket => new TicketHomeViewModel
            {
                Ticket = ticket,
                Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                Log = _logs.GetLastByTicketId(ticket.Id),
                ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

            });
            return View("Home", model);
        }
        //Ticket/Details
        //I dont think this method is used in the application, but I cant remember.  Most requests for a single ticket will come to
        //TicketsByDeviceId as the application primarily goes through the device table to get the ticket.
        public IActionResult Details(int ticketId)
        {
            //get the ticket
            var ticket = _tickets.Get(ticketId);
            //check that it is not null
            if(ticket != null)
            {
                var model = new TicketHomeViewModel
                {
                    Ticket = ticket,
                    Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                    Log = _logs.GetLastByTicketId(ticket.Id),
                    ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                    ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)
                };
                return View("_Home", model);
            }
            //return ticket home if it is null
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        //This code it duplicated from the Device Controller
        public ActionResult GetQRByGuid(Guid code)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            //change to route to site url
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(@"http://chipsmgr.com/Update/Index/" + code.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            byte[] image = BitmapToBytes(qrCodeImage);
            return File(image, "image/jpeg");


        }
        //Method used by the GetQRByGuid Method
        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        //MEthod that displays the confirmation printout page to the customer
        public IActionResult Confirmation(int ticketId, int deviceId, int customerId, Guid updateId)
        {
            var model = new UpdateViewModel
            {
                Ticket = _tickets.Get(ticketId),
                Device = _devices.Get(deviceId),
                Customer = _customers.Get(customerId),
                Update = _updates.Get(updateId)
            };
            return View(model);
        }




    }
}