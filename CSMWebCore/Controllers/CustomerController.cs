using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;
using CSMWebCore.Services;
using CSMWebCore.ViewModels;
using CSMWebCore.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSMWebCore.Controllers
{
    // Check that user is logged in
    [Authorize]
    public class CustomerController : Controller
    {
        private ChipsDbContext context;
        public CustomerController(ChipsDbContext context)
        {
            this.context = context;
        }

        // Customer/Index: Lists all customers
        public IActionResult Index()
        {
            // Get all customers then create an IENumerable of 
            // CustomerViewModel and send to Index View
            var model = context.Customers.Get().Select(cust =>
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

        // Customer/Details: Displays single customer details by ID
        public IActionResult Details(int id)
        {
            // Get the customer object using ID
            var cust = context.Customers.Find(id);
            // Check to see that customer exists
            if(cust == null)
            {
                RedirectToAction("Index");
            }
            // create new CustomerViewModel and send to View
            return View(new NewCustomerDetailsViewModel
            {
                Id = cust.Id,
                FullName = $"{cust.FirstName} {cust.LastName}",
                Email = cust.Email,
                StudentId = cust.StudentId,
                Phone = cust.Phone,
                ContactPref = cust.ContactPref.ToString()
            });
        }

        // Customer/Edit GET: Creates viewmodel for editing customer information              
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // get the customer object
            var cust = context.Customers.Find(id);
            // check to see if it is null
            if(cust == null)
            {
                return RedirectToAction("Index");
            }
            // send customer model to view
            return View(new NewCustomerEditViewModel { 
                FirstName = cust.FirstName,
                LastName = cust.LastName,
                StudentId = cust.StudentId,
                Phone = cust.Phone,
                Email = cust.Email,
                ContactPref = cust.ContactPref,
                Id = cust.Id,
            });
        }
        // Customer/Edit POST: Retrieves viewmodel data from view, maps data to Customer
        // being edited, saves changes, then redirects back to Index action
        [HttpPost]
        public IActionResult Edit(NewCustomerEditViewModel model)
        {
            // Verify the Id that came back from the Post is valid
            var cust = context.Customers.Find(model.Id);
            if(cust == null || !ModelState.IsValid)
            {
                return View(model);
            }
            // update the customer object
            cust.FirstName = model.FirstName;
            cust.LastName = model.LastName;
            cust.StudentId = model.StudentId;
            cust.Email = model.Email;
            cust.Phone = model.Phone;
            cust.ContactPref = model.ContactPref;
            // save changes
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Customer/Create GET: Renders view for creating customer
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        // Customer/Create POST: Saves model data from view to a new Customer object.
        // Redirects to Details page if model is valid, reloads view if not.
        [HttpPost] 
        public IActionResult Create(Customer model)
        {
            // Check to see if model is valid
            if (ModelState.IsValid)
            {
                // create new customer
                Customer cust = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    StudentId = model.StudentId,
                    ContactPref = model.ContactPref
                };
                // add and save changes
                context.Customers.Add(cust);
                context.SaveChanges();
                // the Id of an object recently added to the database
                // is stored in the Id property of the obejct after the
                // changes have been committed.
                int newId = cust.Id;
                // redirect to details sending the newID as a parameter
                return RedirectToAction("Details", new { id = newId });
            }
            // if ModelState failed resent the create View
            return View();
        }

        // Customer/Search: Basic Search Implementation
        public IActionResult Search(string searchValue)
        {
            // Call searchmethod from customer serives and pass searchValue
            // Create a IENumerable of CustomerViewModel, see SQLCustomer for Search Method
            var model = context.Customers.Search(searchValue).Select(cust => new NewCustomerViewModel {
                Id = cust.Id,
                FullName = $"{cust.FirstName} {cust.LastName}",
                Email = cust.Email,
                StudentId = cust.StudentId,
                Phone = cust.Phone,
                ContactPref = cust.ContactPref.ToString()
            });
            // also send the searched for valuue
            ViewBag.SearchValue = searchValue;
            return View(model);
        }

        // Customer/Active: Retrieves a view of customers with active tickets.
        // Method that retrieves all customers with active Tickets
        // This seems innefficient coding wish as I have a ViewModel that
        // Doesnt Contain anything but a customer in it however, the alterntive is
        // to run this context.Customers.Get(context.Devices.Get(ticket.DeviceId).CustomerId).ID, then
        // context.Customers.Get(context.Devices.Get(ticket.DeviceId).CustomerId).FirstName etc.. to create a
        // Customer object instead of the ActiveViewModel and I am guessing this less
        // work for the database and back-end but not sure.

        // Note 4/14:
        // GetOpenTickets extension method returns an IQueryable collection of all tickets that are not closed 
        // (any where most recent log's TicketStatus is not Closed). From there, we select each ticket's customer, 
        // then make sure we only get distinct customers. This collection of customers is then used to generate the viewmodel collection.
        // This means we could use CustomerViewModel instead of needing a dedicated one for active customers.

        public IActionResult Active()
        {
            // get active customers
            IQueryable<Customer> activeCustomers = context.Tickets.GetOpenTickets().Select(t => t.Device.Customer).Distinct();
            // populate viewmodel collection
            IEnumerable<NewCustomerActiveViewModel> viewModel = activeCustomers.Select(customer => new NewCustomerActiveViewModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                Email = customer.Email,
                StudentId = customer.StudentId,
                ContactPref = customer.ContactPref,
                Id = customer.Id
            });
            // pass viewmodel collection to view
            return View(viewModel);
        }
    }
}
