using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Entities;
using CSMWebCore.Services;
using CSMWebCore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CSMWebCore.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerData _customers;
        public CustomerController(ICustomerData customers)
        {
            _customers = customers;
        }

        public IActionResult Index()
        {
            var model = _customers.GetAll().Select(cust =>
            new CustomerViewModel
            {
                Id = cust.Id,
                FirstName = cust.FirstName,
                LastName = cust.LastName,
                Email = cust.Email,
                StudentId = cust.StudentId,
                Phone = cust.Phone,
                ContactPref = cust.ContactPref.ToString()
            });
            return View(model);
        }
        public IActionResult Details(int id)
        {
            var cust = _customers.Get(id);
            if(cust == null)
            {
                RedirectToAction("Index");
            }
            return View(new CustomerViewModel
            {
                Id = cust.Id,
                FirstName = cust.FirstName,
                LastName = cust.LastName,
                Email = cust.Email,
                StudentId = cust.StudentId,
                Phone = cust.Phone,
                ContactPref = cust.ContactPref.ToString()
            });
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cust = _customers.Get(id);
            if(cust == null)
            {
                return RedirectToAction("Index");
            }
            return View(cust);
        }
        [HttpPost]
        public IActionResult Edit(CustomerEditViewModel model)
        {
            var cust = _customers.Get(model.Id);
            if(cust == null || !ModelState.IsValid)
            {
                return View(model);
            }
            cust.FirstName = model.FirstName;
            cust.LastName = model.LastName;
            cust.StudentId = model.StudentId;
            cust.Email = model.Email;
            cust.Phone = model.Phone;
            cust.ContactPref = model.ContactPref;
            _customers.Commit();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost] 
        public IActionResult Create(CustomerEditViewModel model)
        {
            Random rand = new Random();
            model.StudentId = rand.Next(100000000, 999999999).ToString();
            model.Phone = rand.Next(1000000000, 1999999999).ToString();
            if (ModelState.IsValid)
            {
                Customer cust = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    StudentId = model.StudentId,
                    ContactPref = model.ContactPref
                };
                _customers.Add(cust);
                _customers.Commit();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}