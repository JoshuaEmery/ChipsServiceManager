using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSMWebCore.Controllers
{
    public class ReportController : Controller
    {
        private IReportsService _rs;
        public ReportController(IReportsService rs)
        {
            _rs = rs;
        }
        
        public IActionResult Index()
        {
            
            return View();
        }
    }
}