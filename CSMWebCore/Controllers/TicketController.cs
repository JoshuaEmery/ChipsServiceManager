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
    [Authorize]
    public class TicketController : Controller
    {
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;
        private IUpdateData _updates;

        // constructor with dependency injection for all five tables
        public TicketController(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs, IUpdateData updates)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
            _updates = updates;
        }

        //Ticket/Index
        public IActionResult Index()
        {
            //create an IEnumerable of TicketViewModel from the active tickets IENumerable
            var model = _tickets.GetAllActiveTickets().Select(ticket => new TicketViewModel
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

        [HttpGet]
        //Ticket/CreateByExistingDeviceId
        // create ticket for existing device (and thus existing customer)
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
                Customer = _customers.Get(device.CustomerId),
                CustomerId = device.CustomerId

            };
            //Get the current Ticket Number
            model.Ticket.TicketNumber = _tickets.CurrentTicketNumber() + 1;
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateByExistingDeviceId(DeviceEditViewModel model)
        {
            //Get Ticket Number again and check ModelState
            model.Ticket.TicketNumber = _tickets.CurrentTicketNumber() + 1;
            if (!ModelState.IsValid)
            {
                model.Customer = _customers.Get(model.CustomerId);
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
        public IActionResult Edit(int id)
        {
            var model = _tickets.Get(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
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
            return RedirectToAction("Index");

        }

        //Ticket/Details
        // TODO review usage, since tickets can be accessed via TicketsByDeviceId
        public IActionResult Details(int ticketId)
        {
            //get the ticket
            var ticket = _tickets.Get(ticketId);
            //check that it is not null
            if (ticket != null)
            {
                var model = new TicketViewModel
                {
                    Ticket = ticket,
                    Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                    Log = _logs.GetLastByTicketId(ticket.Id),
                    ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                    ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)
                };
                return View("_Index", model);
            }
            //return ticket Index if it is null
            return RedirectToAction("Index");
        }

        // -- SORTING, FILTERING, SEARCHING

        //Ticket/FilterByStatus
        // called by status dropdown in Index view; selects relevant tickets for model and
        // populates TicketViewModel DateFilter with corresp. enum value
        [HttpPost]
        public IActionResult FilterByStatus(TicketViewModel result)
        {
            //If they chose all show the default Ticket/Index View
            if (result.Status == "All")
            {
                return RedirectToAction("Index");
            }
            //Create IEnumerable of Tickets that match the Ticketstatus given
            var model = _tickets.GetByStatus(result.TicketStatus).Select(ticket => new TicketViewModel
            {
                Ticket = ticket,
                Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                Log = _logs.GetLastByTicketId(ticket.Id),
                ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)
            });
            //return
            return View("Index", model);
        }

        //Ticket/FilterByDate
        // called by date dropdown in Index view; sorts model and populates TicketViewModel 
        // DateFilter with corresp. enum value
        [HttpPost]
        public IActionResult FilterByDate(TicketViewModel result)
        {
            //If all return default Ticket/Index View
            if (result.Status == "All")
            {
                return RedirectToAction("Index");
            }
            //Check which filter was used and return corresponsding IENumerable of TicketViewModel
            else if (result.DateFilter == DateFilter.Oldest)
            {
                var model = _tickets.GetAll().OrderBy(x => x.CheckedIn).Select(ticket => new TicketViewModel
                {
                    Ticket = ticket,
                    Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                    Log = _logs.GetLastByTicketId(ticket.Id),
                    ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                    ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

                });

                return View("Index", model);
            }
            else if (result.DateFilter == DateFilter.Newest)
            {
                var model = _tickets.GetAll().OrderByDescending(x => x.CheckedIn).Select(ticket => new TicketViewModel
                {
                    Ticket = ticket,
                    Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                    Log = _logs.GetLastByTicketId(ticket.Id),
                    ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                    ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

                });

                return View("Index", model);
            }
            else if (result.DateFilter == DateFilter.Idle)
            {
                var model = _tickets.GetAll().OrderBy(x => x.CheckedIn).Select(ticket => new TicketViewModel
                {
                    Ticket = ticket,
                    Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                    Log = _logs.GetLastByTicketId(ticket.Id),
                    ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                    ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id),
                    DaysIdle = DateTime.Now - _logs.GetLastByTicketId(ticket.Id).Logged
                });
                var sorted = model.OrderByDescending(x => x.DaysIdle).ToList();

                return View("Index", sorted);
            }
            //this should never run, but if it does return default view
            return View("Index");
        }

        //Ticket/TicketsByDeviceId
        // gets tickets for device given id
        public IActionResult TicketsByDeviceId(int deviceId)
        {
            //Create an IEnumerable of TicketViewModel that match the given deviceID
            var model = _tickets.GetAllByDevice(deviceId).Select(ticket => new TicketViewModel
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
                return View("Index", model);
            }
            return View();
        }

        //Ticket/Search
        // TODO review usage, currently only searches Ticket fields incl. Ticket No. and checkin technician.
        // Customer or Device search has more functionality
        public IActionResult Search(string searchValue)
        {
            //Create an IEnumerable of TicketViewModel that match the searchvalue.  Check SQLTicket for
            //search method
            var model = _tickets.Search(searchValue).Select(ticket => new TicketViewModel
            {
                Ticket = ticket,
                Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                Log = _logs.GetLastByTicketId(ticket.Id),
                ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

            });
            return View("Index", model);
        }

        // methods for QR code generation and printout page view for customer
        [Authorize]
        // returns QR code jpeg (duplicate code from DeviceController)
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
        // converts bitmap to byte array
        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        // displays Update view for printout
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


        //Ticket/OldIndex
        // Early testing method
        //public IActionResult OldIndex()
        //{
        //    var model = _tickets.GetAll().Select(cust => new TicketOldIndexViewModel
        //    {
        //        Id = cust.Id,
        //        DeviceId = cust.DeviceId,
        //        CheckedIn = cust.CheckedIn.ToShortDateString(),
        //        CheckedOut = cust.CheckedOut.ToShortDateString(),
        //        Finished = cust.Finished.ToShortDateString(),
        //        CheckInUserId = cust.CheckInUserId,
        //        CheckOutUserId = cust.CheckOutUserId,
        //        NeedsBackup = cust.NeedsBackup,
        //        TicketStatus = cust.TicketStatus.ToString()
        //    });
        //    return View(model);
        //}

    }
}