using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Services;
using CSMWebCore.ViewModels;
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
        [HttpGet]
        public IActionResult FoundationReport()
        {
            return View(new FoundationReportViewModel
            {
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date,
                Report = "TESTTEST"
            });
        }
        [HttpPost]
        public IActionResult FoundationReport(FoundationReportViewModel model)
        {
            return Content($"{model.StartDate.Date.ToString()} {model.EndDate.Date.ToString()}");
        }
    }
}
