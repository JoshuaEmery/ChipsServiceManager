using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}