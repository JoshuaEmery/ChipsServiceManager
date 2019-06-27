using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Services;
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
            return View();
        }
    }
}