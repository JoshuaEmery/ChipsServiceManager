using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CSMWebCore.Data;
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
        //private IDeviceRepository _devices;
        //private ICustomerRepository _customers;
        //private ITicketRepository _tickets;
        //private ILogRepository _logs;
        private IUpdateData _updates;
        private ITicketCreator _ticketCreator;
        private ChipsDbContext ctx;
        //private UnitOfWork unitOfWork = new UnitOfWork();

        public TicketController(ChipsDbContext context, IUpdateData updates, ITicketCreator ticketCreator)
        {
            //_devices = devices;
            //_customers = customers;
            //_tickets = tickets;
            //_logs = logs;
            ctx = context;
            _updates = updates;
            _ticketCreator = ticketCreator;
        }

        //Ticket/Index
        public IActionResult Index()
        {
            //create an IEnumerable of TicketViewModel from open tickets (any with status not Closed)
            var model = ctx.Tickets.Select(ticket => new TicketViewModel
            {
                Id = ticket.Id,
                TicketNumber = ticket.TicketNumber,
                NeedsBackup = ticket.NeedsBackup,
                CustomerId = ticket.Device.Customer.Id,
                CustomerFName = ticket.Device.Customer.FirstName,
                CustomerLName = ticket.Device.Customer.LastName,
                DeviceId = ticket.Device.Id,
                DeviceMake = ticket.Device.Make,
                DeviceModelNumber = ticket.Device.ModelNumber,
                DeviceOS = ticket.Device.OperatingSystem,
                LogOpenDate = ticket.Logs.Min(l => l.DateCreated),
                LogLatestStatus = ticket.Logs.OrderBy(l => l.DateCreated).Last().TicketStatus,
                LogLatestDate = ticket.Logs.Max(l => l.DateCreated),
                LogCloseDate = ticket.Logs.Max(l => l.DateCreated),
                //LogCloseDate = ticket.Logs.FirstOrDefault(l => l.TicketStatus == TicketStatus.Closed).DateCreated
            });
            return View(model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TicketDeviceCustCreateVM model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    StudentId = model.StudentId,
                    ContactPref = model.ContactPref
                };
                Device device = new Device()
                {
                    Customer = customer,
                    Make = model.Make,
                    ModelNumber = model.ModelNumber,
                    OperatingSystem = model.OperatingSystem,
                    Password = model.Password,
                    Serviced = model.Serviced
                };
                Ticket ticket = new Ticket()
                {
                    Device = device,
                    TicketNumber = ctx.Tickets.Max(t => t.TicketNumber) + 1,
                    NeedsBackup = model.NeedsBackup
                };
                Log log = new Log()
                {
                    Ticket = ticket,
                    Event = ctx.Events.Single(e => e.Id == 1),
                    TicketStatus = TicketStatus.New,
                    UserCreated = "chippy",
                    DateCreated = DateTime.Now,
                    Notes = model.Notes
                };
                ctx.Logs.Add(log); // stages all four entities for adding to db
                ctx.SaveChanges();
            }
            return RedirectToAction("Index", "Ticket");          
        }


        //[HttpGet]
        ////Ticket/CreateByExistingDeviceId
        //// create ticket for existing device (and thus existing customer)
        //public IActionResult CreateByExistingDeviceId(int deviceId)
        //{
        //    //get the device and check it
        //    var device = _devices.GetById(deviceId);
        //    if (device == null)
        //    {
        //        return View();
        //    }
        //    //If it exists create ViewModel and send it to the View
        //    DeviceEditViewModel model = new DeviceEditViewModel
        //    {
        //        Id = device.Id,
        //        Make = device.Make,
        //        ModelNumber = device.ModelNumber,
        //        OperatingSystem = device.OperatingSystem,
        //        Password = device.Password,
        //        Serviced = device.Serviced,
        //        Ticket = new Ticket(),
        //        Customer = _customers.GetById(device.CustomerId),
        //        CustomerId = device.CustomerId
        //    };
        //    //Get the current Ticket Number
        //    model.Ticket.TicketNumber = _tickets.GetLatestTicketNum() + 1;
        //    return View(model);
        //}
        //[HttpPost]
        //public IActionResult CreateByExistingDeviceId(DeviceEditViewModel model)
        //{
        //    //Get Ticket Number again and check ModelState
        //    model.Ticket.TicketNumber = _tickets.GetLatestTicketNum() + 1;
        //    if (!ModelState.IsValid)
        //    {
        //        model.Customer = _customers.GetById(model.CustomerId);
        //        return View(model);

        //    }
        //    //get the device and create a new ticket from the device
        //    var device = _devices.GetById(model.Id);
        //    TicketConfirmationModel tcModel = _ticketCreator.CreateTicket(new TicketCreatorInfo
        //    {
        //        DeviceId = device.Id,
        //        CustomerId = device.CustomerId,
        //        NeedsBackup = model.Ticket.NeedsBackup,
        //        Notes = model.Log.Notes,
        //        UserName = User.FindFirst(ClaimTypes.Name).Value.ToString()
        //    });
        //    return RedirectToAction("Confirmation", "Ticket", new
        //    {
        //        ticketId = tcModel.ticketId,
        //        deviceId = tcModel.deviceId,
        //        customerId = tcModel.customerId,
        //        updateId = tcModel.updateId
        //    });
        //}

        //[HttpGet]
        ////Ticket/Edit
        //public IActionResult Edit(int id)
        //{
        //    var model = _tickets.GetById(id);
        //    if (model == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View(model);
        //}
        //[HttpPost]
        //public IActionResult Edit(TicketEditViewModel model)
        //{
        //    //get the ticket to edit
        //    var ticket = _tickets.GetById(model.Id);
        //    if (ticket == null || !ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    //edit the ticket
        //    ticket.DeviceId = model.DeviceId;
        //    ticket.CheckInDate = model.CheckedIn;
        //    ticket.CheckOutDate = model.CheckedOut;
        //    ticket.FinishDate = model.Finished;
        //    ticket.CheckInUserId = model.CheckInUserId;
        //    ticket.CheckOutUserId = model.CheckOutUserId;
        //    ticket.NeedsBackup = model.NeedsBackup;
        //    ticket.Status = model.TicketStatus;
        //    //save changes
        //    _tickets.Commit();
        //    return RedirectToAction("Index");

        //}

        ////Ticket/Details
        //// TODO review usage, since tickets can be accessed via TicketsByDeviceId
        //public IActionResult Details(int ticketId)
        //{
        //    //get the ticket
        //    var ticket = _tickets.GetById(ticketId);
        //    //check that it is not null
        //    if (ticket != null)
        //    {
        //        var model = new TicketViewModel
        //        {
        //            Ticket = ticket,
        //            Customer = _customers.GetById(_devices.GetById(ticket.DeviceId).CustomerId),
        //            Log = _logs.GetLastByTicketId(ticket.Id),
        //            ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
        //            ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)
        //        };
        //        return View("_Index", model);
        //    }
        //    //return ticket Index if it is null
        //    return RedirectToAction("Index");
        //}

        //// -- SORTING, FILTERING, SEARCHING

        ////Ticket/FilterByStatus
        //// called by status dropdown in Index view; selects relevant tickets for model and
        //// populates TicketViewModel DateFilter with corresp. enum value
        //[HttpPost]
        //public IActionResult FilterByStatus(TicketViewModel result)
        //{
        //    //If they chose all show the default Ticket/Index View
        //    if (result.Status == "All")
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    //Create IEnumerable of Tickets that match the Ticketstatus given
        //    var model = _tickets.GetByStatus(result.TicketStatus).Select(ticket => new TicketViewModel
        //    {
        //        Ticket = ticket,
        //        Customer = _customers.GetById(_devices.GetById(ticket.DeviceId).CustomerId),
        //        Log = _logs.GetLastByTicketId(ticket.Id),
        //        ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
        //        ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)
        //    });
        //    //return
        //    return View("Index", model);
        //}

        ////Ticket/FilterByDate
        //// called by date dropdown in Index view; sorts model and populates TicketViewModel 
        //// DateFilter with corresp. enum value
        //[HttpPost]
        //public IActionResult FilterByDate(TicketViewModel result)
        //{
        //    //If all return default Ticket/Index View
        //    if (result.Status == "All")
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    //Check which filter was used and return corresponsding IENumerable of TicketViewModel
        //    else if (result.DateFilter == DateFilter.Oldest)
        //    {
        //        var model = _tickets.Get().OrderBy(x => x.CheckInDate).Select(ticket => new TicketViewModel
        //        {
        //            Ticket = ticket,
        //            Customer = _customers.GetById(_devices.GetById(ticket.DeviceId).CustomerId),
        //            Log = _logs.GetLastByTicketId(ticket.Id),
        //            ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
        //            ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

        //        });

        //        return View("Index", model);
        //    }
        //    else if (result.DateFilter == DateFilter.Newest)
        //    {
        //        var model = _tickets.Get().OrderByDescending(x => x.CheckInDate).Select(ticket => new TicketViewModel
        //        {
        //            Ticket = ticket,
        //            Customer = _customers.GetById(_devices.GetById(ticket.DeviceId).CustomerId),
        //            Log = _logs.GetLastByTicketId(ticket.Id),
        //            ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
        //            ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

        //        });

        //        return View("Index", model);
        //    }
        //    else if (result.DateFilter == DateFilter.Idle)
        //    {
        //        var model = _tickets.Get().OrderBy(x => x.CheckInDate).Select(ticket => new TicketViewModel
        //        {
        //            Ticket = ticket,
        //            Customer = _customers.GetById(_devices.GetById(ticket.DeviceId).CustomerId),
        //            Log = _logs.GetLastByTicketId(ticket.Id),
        //            ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
        //            ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id),
        //            DaysIdle = DateTime.Now - _logs.GetLastByTicketId(ticket.Id).DateCreated
        //        });
        //        var sorted = model.OrderByDescending(x => x.DaysIdle).ToList();

        //        return View("Index", sorted);
        //    }
        //    //this should never run, but if it does return default view
        //    return View("Index");
        //}

        ////Ticket/TicketsByDeviceId
        //// gets tickets for device given id
        //public IActionResult TicketsByDeviceId(int deviceId)
        //{
        //    //Create an IEnumerable of TicketViewModel that match the given deviceID
        //    var model = _tickets.GetAllByDevice(deviceId).Select(ticket => new TicketViewModel
        //    {
        //        Ticket = ticket,
        //        Customer = _customers.GetById(_devices.GetById(ticket.DeviceId).CustomerId),
        //        Log = _logs.GetLastByTicketId(ticket.Id),
        //        ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
        //        ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

        //    });
        //    //Check if model is null
        //    if (model != null)
        //    {
        //        return View("Index", model);
        //    }
        //    return View();
        //}

        ////Ticket/Search
        //// TODO review usage, currently only searches Ticket fields incl. Ticket No. and checkin technician.
        //// Customer or Device search has more functionality
        //public IActionResult Search(string searchValue)
        //{
        //    //Create an IEnumerable of TicketViewModel that match the searchvalue.  Check SQLTicket for
        //    //search method
        //    var model = _tickets.Search(searchValue).Select(ticket => new TicketViewModel
        //    {
        //        Ticket = ticket,
        //        Customer = _customers.GetById(_devices.GetById(ticket.DeviceId).CustomerId),
        //        Log = _logs.GetLastByTicketId(ticket.Id),
        //        ServiceLogs = _logs.GetServiceLogsByTicketId(ticket.Id),
        //        ContactLogs = _logs.GetContactLogsByTicketId(ticket.Id)

        //    });
        //    return View("Index", model);
        //}

        //// methods for QR code generation and printout page view for customer
        //[Authorize]
        //// returns QR code jpeg (duplicate code from DeviceController)
        //public ActionResult GetQRByGuid(Guid code)
        //{
        //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //    //change to route to site url
        //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(@"http://chipsmgr.com/TicketProgress/Index/" + code.ToString(), QRCodeGenerator.ECCLevel.Q);
        //    QRCode qrCode = new QRCode(qrCodeData);
        //    Bitmap qrCodeImage = qrCode.GetGraphic(20);
        //    byte[] image = BitmapToBytes(qrCodeImage);
        //    return File(image, "image/jpeg");
        //}
        //// converts bitmap to byte array
        //private static byte[] BitmapToBytes(Bitmap img)
        //{
        //    using MemoryStream stream = new MemoryStream();
        //    img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //    return stream.ToArray();
        //}
        //// displays Update view for printout
        //public IActionResult Confirmation(int ticketId, int deviceId, int customerId, Guid updateId)
        //{
        //    var model = new ConfirmationViewModel
        //    {
        //        Ticket = _tickets.GetById(ticketId),
        //        Device = _devices.GetById(deviceId),
        //        Customer = _customers.GetById(customerId),
        //        Update = _updates.Get(updateId)
        //    };
        //    return View(model);
        //}
    }
}