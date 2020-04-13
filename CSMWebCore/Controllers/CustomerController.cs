using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Entities;
using CSMWebCore.Services;
using CSMWebCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSMWebCore.Controllers
{
    //Check that user is looged in
    [Authorize]
    public class CustomerController : Controller
    {
        //Customers controller needs access to customers, tickets and devices tables
        private ICustomerRepository _customers;
        private ITicketRepository _tickets;
        private IDeviceRepository _devices;
        //Constructor
        public CustomerController(ICustomerRepository customers, ITicketRepository tickets, IDeviceRepository devices)
        {
            _customers = customers;
            _tickets = tickets;
            _devices = devices;
        }
        //Customer/Index
        //Index Method Lists all Customers
        public IActionResult Index()
        {
            //Get all customers then create an IENumerable of 
            //CustomerViewModel and send to Index View
            var model = _customers.Get().Select(cust =>
            new CustomerViewModel
            {
                Id = cust.Id,
                FullName = $"{cust.FirstName} {cust.LastName}",
                Email = cust.Email,
                StudentId = cust.StudentId,
                Phone = cust.Phone,
                ContactPref = cust.ContactPref.ToString()
            });
            return View(model);
        }
        //Customer/Details
        //Get a customer with a specific ID
        public IActionResult Details(int id)
        {
            //Get the customer object using ID
            var cust = _customers.GetById(id);
            //Check to see that customer exists
            if(cust == null)
            {
                RedirectToAction("Index");
            }
            //create new CustomerViewModel and send to View
            return View(new CustomerViewModel
            {
                Id = cust.Id,
                FullName = $"{cust.FirstName} {cust.LastName}",
                Email = cust.Email,
                StudentId = cust.StudentId,
                Phone = cust.Phone,
                ContactPref = cust.ContactPref.ToString()
            });
        }
        //Customer/Edit
        //HttpGet for Edit
        //Use of the actual customer entity vs the CustomerViewModel                  
        [HttpGet]
        public IActionResult Edit(int id)
        {
            //get the customer object
            var cust = _customers.GetById(id);
            //check to see if it is null
            if(cust == null)
            {
                return RedirectToAction("Index");
            }
            //send customer model to view
            return View(cust);
        }
        //Post Method for Edit
        [HttpPost]
        public IActionResult Edit(CustomerEditViewModel model)
        {
            //Verify the Id that came back from the Post is valid
            var cust = _customers.GetById(model.Id);
            if(cust == null || !ModelState.IsValid)
            {
                return View(model);
            }
            //update the customer object
            cust.FirstName = model.FirstName;
            cust.LastName = model.LastName;
            cust.StudentId = model.StudentId;
            cust.Email = model.Email;
            cust.Phone = model.Phone;
            cust.ContactPref = model.ContactPref;
            //save changes
            _customers.Commit();
            return RedirectToAction("Index");
        }
        //Customer/Create
        //Get for Create Customer
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        //Post for Customer
        [HttpPost] 
        public IActionResult Create(CustomerEditViewModel model)
        {
            //Check to see if model is valid
            if (ModelState.IsValid)
            {
                //create new customer
                Customer cust = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    StudentId = model.StudentId,
                    ContactPref = model.ContactPref
                };
                //add and save changes
                _customers.Insert(cust);
                _customers.Commit();
                //the Id of an object recently added to the database
                //is stored in the Id property of the obejct after the
                //changes have been committed.
                int newId = cust.Id;
                //redirect to details sending the newID as a parameter
                return RedirectToAction("Details", new { id = newId });
            }
            //if ModelState failed resent the create View
            return View();
        }
        //Customer/Search
        //Basic Search Implementation
        public IActionResult Search(string searchValue)
        {
            //Call searchmethod from customer serives and pass searchValue
            //Create a IENumerable of CustomerViewModel, see SQLCustomer for Search Method
            var model = _customers.Search(searchValue).Select(cust => new CustomerViewModel {
                Id = cust.Id,
                FullName = $"{cust.FirstName} {cust.LastName}",
                Email = cust.Email,
                StudentId = cust.StudentId,
                Phone = cust.Phone,
                ContactPref = cust.ContactPref.ToString()
            });
            //also send the searched for valuue
            ViewBag.SearchValue = searchValue;
            return View(model);
        }
        //Customer/Active
        //Method that retrieves all customers with active Tickets
        //This seems innefficient coding wish as I have a ViewModel that
        //Doesnt Contain anything but a customer in it however, the alterntive is
        //to run this _customers.Get(_devices.Get(ticket.DeviceId).CustomerId).ID, then
        //_customers.Get(_devices.Get(ticket.DeviceId).CustomerId).FirstName etc.. to create a
        //Customer object instead of the ActiveViewModel and I am guessing this less
        //work for the database and back-end but not sure.
        public IActionResult Active()
        {
            //the model will be a list of CustomerActiveViewModel that is created from a list
            //of Tickets that are active
            var model = _tickets.GetOpen().Select(ticket => new CustomerActiveViewModel
            {
                //Retrieve each Customer by getting the CustomerID stored in the Device that is
                //stored in the Ticket
                Customer = _customers.GetById(_devices.GetById(ticket.Device.Id).CustomerId)
            });
            //return View
            return View(model);
        }
    }
}