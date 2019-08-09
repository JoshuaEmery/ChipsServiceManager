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

        public TicketController(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs, IUpdateData updates)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
            _updates = updates;
        }
        //Begin Test Actions
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
        [HttpGet]
        public IActionResult CreateByExistingDeviceId(int deviceId)
        {
            var device = _devices.Get(deviceId);
            if (device == null)
            {
                return View();
            }
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
            model.Ticket.TicketNumber = _tickets.CurrentTicketNumber() + 1;
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateByExistingDeviceId(DeviceEditViewModel model)
        {
            if (model.Ticket.TicketNumber == _tickets.CurrentTicketNumber())
            {
                return View("Confirmation");
            }
            if (!ModelState.IsValid)
            {
                model.Owner = _customers.Get(model.CustomerId);
                return View(model);

            }
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
            _tickets.Add(ticket);
            _tickets.Commit();
            Log log = new Log
            {
                UserId = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                TicketId = ticket.Id,
                Logged = DateTime.Now,
                Notes = model.Log.Notes,
                LogType = LogType.CheckIn,
                ContactMethod = ContactMethod.InPerson
            };
            _logs.Add(log);
            _logs.Commit();

            Update update = new Update
            {
                Id = new Guid(),
                TicketId = ticket.Id
            };
            _updates.Add(update);
            _updates.Commit();
            var updateViewModel = new UpdateViewModel
            {
                Ticket = ticket,
                Device = device,
                Customer = _customers.Get(model.CustomerId),
                Update = update
            };

            return View("Confirmation", updateViewModel);
        }
        [HttpGet]
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
            var ticket = _tickets.Get(model.Id);
            if (ticket == null || !ModelState.IsValid)
            {
                return View(model);
            }
            ticket.DeviceId = model.DeviceId;
            ticket.CheckedIn = model.CheckedIn;
            ticket.CheckedOut = model.CheckedOut;
            ticket.Finished = model.Finished;
            ticket.CheckInUserId = model.CheckInUserId;
            ticket.CheckOutUserId = model.CheckOutUserId;
            ticket.NeedsBackup = model.NeedsBackup;
            ticket.TicketStatus = model.TicketStatus;
            _tickets.Commit();
            return RedirectToAction("Home");

        }

        //End Test Actions
        //Begin Launch Actions

        public IActionResult Home()
        {
            
            var model = _tickets.GetAllActiveTickets().Select(ticket => new TicketHomeViewModel
            {
                Ticket = ticket,
                Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                Log = _logs.GetLastByTicketId(ticket.Id),
                ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

            });

            return View(model);
        }
        [HttpPost]
        public IActionResult FilterByStatus(TicketHomeViewModel result)
        {
            if (result.Status == "All")
            {
                return RedirectToAction("Home");
            }
            var model = _tickets.GetByStatus(result.TicketStatus).Select(ticket => new TicketHomeViewModel
            {
                Ticket = ticket,
                Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                Log = _logs.GetLastByTicketId(ticket.Id),
                ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

            });

            return View("Home", model);
        }
        [HttpPost]
        public IActionResult FilterByDate(TicketHomeViewModel result)
        {
            if (result.Status == "All")
            {
                return RedirectToAction("Home");
            }
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

            return View("Home");
        }
        public IActionResult TicketsByDeviceId(int deviceId)
        {
            var model = _tickets.GetAllByDevice(deviceId).Select(ticket => new TicketHomeViewModel
            {
                Ticket = ticket,
                Customer = _customers.Get(_devices.Get(ticket.DeviceId).CustomerId),
                Log = _logs.GetLastByTicketId(ticket.Id),
                ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
                ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

            });
            if (model != null)
            {
                return View("Home",model);
            }
            return View();



        }       
        public IActionResult Search(string searchValue)
        {

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
        public IActionResult Details(int ticketId)
        {
            var ticket = _tickets.Get(ticketId);
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
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult GetQRByGuid(Guid code)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(@"https://csmwebcore.conveyor.cloud/update/index/" + code.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            var image = BitmapToBytes(qrCodeImage);
            return File(image, "image/jpeg");


        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }




    }
}