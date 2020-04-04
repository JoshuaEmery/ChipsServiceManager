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
using Microsoft.AspNetCore.Mvc.Rendering;
using QRCoder;

namespace CSMWebCore.Controllers
{
    //Check for Login
    [Authorize]
    public class DeviceController : Controller
    {
        //Device Controller uses all entity tables
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;
        private IUpdateData _updates;
        //constructor
        public DeviceController(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs, IUpdateData updates)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
            _updates = updates;
        }
        //Device/Index
        // returns view for list of devices assigned to *active* tickets
        public IActionResult Index()
        {
            // create list
            List<DeviceViewModel> model = new List<DeviceViewModel>();
            //Get all active tickets
            var activetickets = _tickets.GetAllActiveTickets();
            // sequence thru active tickets, add viewmodel for each device to list     
            foreach (var ticket in activetickets)
            {
                var device = _devices.Get(ticket.DeviceId);
                model.Add(new DeviceViewModel
                {
                    Id = device.Id,
                    Customer = _customers.Get(device.CustomerId),
                    Make = device.Make,
                    ModelNumber = device.ModelNumber,
                    OperatingSystem = device.OperatingSystem,
                    Password = device.Password,
                    Serviced = device.Serviced,
                    Ticket = ticket
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
            var device = _devices.Get(deviceId);
            if (device == null)
            {
                return View();
            }
            //create new DeviceEditViewModel
            DeviceEditViewModel model = new DeviceEditViewModel
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
        public IActionResult Edit(DeviceEditViewModel model)
        {
            //Check to see that the device exists
            var device = _devices.Get(model.Id);
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
            _devices.Commit();
            return RedirectToAction("Index");
        }
        //Device/Create - This Method Used for Testing.  No Longer necessary, use CreatebyCustomerID
        //instead
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    //
        //    DeviceEditViewModel model = new DeviceEditViewModel();
        //    model.Customer = new List<SelectListItem>();
        //    foreach (var customer in _customers.GetAll())
        //    {
        //        model.Customer.Add(new SelectListItem(customer.FirstName + " " + customer.LastName, customer.Id.ToString()));
        //    }
        //    return View(model);
        //}
        //[HttpPost]
        //public IActionResult Create(DeviceEditViewModel model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        Device device = new Device
        //        {
        //            CustomerId = model.CustomerId,
        //            Make = model.Make,
        //            ModelNumber = model.ModelNumber,
        //            OperatingSystem = model.OperatingSystem,
        //            Password = model.Password,
        //            Serviced = model.Serviced
        //        };
        //        _devices.Add(device);
        //        _devices.Commit();
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}
        //Device/Detials
        //Method that Shows the details of a device by ID
        public IActionResult Details(int deviceId)
        {
            //Check if device exists
            var device = _devices.Get(deviceId);
            if (device == null)
            {
                return View();
            }
            //create and return ViewModel
            var model = new DeviceEditViewModel
            {
                Id = device.Id,
                Customer = _customers.Get(device.CustomerId),
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
        public IActionResult CreateByCustId(int customerId)
        {
            //The DeviceEditViewModel stores a Ticket, a Customer and TicketID and CustomerID
            //On get Customer is populated, on Post Ticket is populated, the customer will
            //have to be retrieved again if needed on post.
            DeviceEditViewModel model = new DeviceEditViewModel();
            model.Ticket = new Ticket();
            model.CustomerId = customerId;
            model.Customer = _customers.Get(customerId);
            //Get the next ticketnumber, this check is run again after post
            model.Ticket.TicketNumber = _tickets.CurrentTicketNumber() + 1;
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
        public IActionResult CreateByCustId(DeviceEditViewModel model)
        {
            //Check if Model State is Valid
            if (!ModelState.IsValid)
            {
                //Get the customer object again from the database as no customer
                //is posted back from the method
                model.Customer = _customers.Get(model.CustomerId);
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
            //Save the new devicesa
            _devices.Add(device);
            _devices.Commit();
            //Check that the ticketNumber has not changed
            model.Ticket.TicketNumber = _tickets.CurrentTicketNumber() + 1;
            //Create a new ticket with the device ID as a foreign key from the newly saved device object
            Ticket ticket = new Ticket
            {                
                DeviceId = device.Id,
                //Gets a string representation of the ChipsUser currently logged in
                CheckInUserId = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                CheckedIn = DateTime.Now,
                NeedsBackup = model.Ticket.NeedsBackup,
                TicketStatus = TicketStatus.New,
                TicketNumber = model.Ticket.TicketNumber
            };
            //Save the new Ticket
            _tickets.Add(ticket);
            _tickets.Commit();
            //Create a log entry with the newly created Ticket.Id as a foreign key
            Log log = new Log
            {                
                UserId = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                TicketId = ticket.Id,
                Logged = DateTime.Now,
                Notes = model.Log.Notes,
                LogType = LogType.CheckIn,
                ContactMethod = ContactMethod.InPerson
            };
            //Add new Log
            _logs.Add(log);
            _logs.Commit();
            //Create a new entry in the update table with a guid for the Primary Key and
            //a foreign key from the Ticket
            Update update = new Update
            {
                Id = new Guid(),
                TicketId = ticket.Id
            };
            //Save Changes
            _updates.Add(update);
            _updates.Commit();
            //Redirect to Confirmation Pags with the new Primary keys of the created objects
            return RedirectToAction("Confirmation", new { ticketId = ticket.Id,
            deviceId = device.Id,
            customerId = model.CustomerId,
            updateId = update.Id});
        }
        //Device/DevicesByCustId
        //Method that gets all devices owned by a given customer
        [HttpGet]
        public IActionResult DevicesByCustId(int id)
        {
            //check to see if customer exists
            var cust = _customers.Get(id);
            if (cust == null)
            {
                return View();
            }
            //ViewBag used to display Customer Name
            ViewBag.Customer = cust;
            //create a IEnumerable of DeviceViewModel by customer ID
            var model = _devices.GetAllByCustId(id).Select(device => new DeviceViewModel
            {
                Id = device.Id,
                Customer = device.Customer,
                Make = device.Make,
                ModelNumber = device.ModelNumber,
                OperatingSystem = device.OperatingSystem,
                Password = device.Password,
                Serviced = device.Serviced

            });
            return View(model);
        }
        //Extra Authorize here, not sure why but now scared to remove
        //Method that takes a Guid and generates an image of a QRCode with the chips
        //URL concatinated with the GUID.  This method is called directly by the
        //Confirmation view and is given a file result.
        [Authorize]
        public ActionResult GetQRByGuid(Guid code)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            //change to route to site url
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(@"http://chipsmgr.com/Update/Index/" + code.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            byte[] image = BitmapToBytes(qrCodeImage);
            //this return sends a byte[] as well as a string representation of
            //the file type of the byte[]
            return File(image, "image/jpeg");
        }
        //This method is used by GetQRByCode and takes a BitMap
        //and returns that same image as a byte array, which for whatever
        //reason is what needs to be sent to the view.
        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        //Device/Search
        public IActionResult Search(string searchValue)
        {
            //create an IEnumerable of DeviceViewModel from using the searchValue, 
            //see SQLDevice for Search method
            var model = _devices.Search(searchValue).Select(device => new DeviceViewModel
            {
                Id = device.Id,
                Customer = _customers.Get(device.CustomerId),
                Make = device.Make,
                ModelNumber = device.ModelNumber,
                OperatingSystem = device.OperatingSystem,
                Password = device.Password,
                Serviced = device.Serviced,
                Ticket = _tickets.GetRecentByDevice(device.Id)                

            });
            return View("Index", model);
        }
        //Device/Confirmation
        //Method gets the data needed for the confirmation view
        //The confirmation view is the printout for the customer on checkin
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