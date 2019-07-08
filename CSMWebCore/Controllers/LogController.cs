using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CSMWebCore.Entities;
using CSMWebCore.Services;
using CSMWebCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSMWebCore.Controllers
{
    [Authorize]
    public class LogController : Controller
    {
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;

        public LogController(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(LogEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Log log = new Log
                {
                    UserId = User.FindFirst(ClaimTypes.Name).Value.ToString(),
                    TicketId = model.TicketId,
                    Logged = DateTime.Now,
                    Notes = model.Notes,
                    LogType = model.LogType,
                    GetContactMethod = model.GetContactMethod,
                };
                _logs.Add(log);
                _logs.Commit();
                return RedirectToAction("Index");
            }
            return View();
            
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _logs.Get(id);
            if (model == null)
            {
                return View();
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(LogEditViewModel model)
        {
            var log = _logs.Get(model.Id);
            if (log == null || !ModelState.IsValid)
            {
                return View();
            }
            log.UserId = model.UserId;
            log.TicketId = model.TicketId;
            log.Logged = model.Logged;
            log.Notes = model.Notes;
            log.LogType = model.LogType;
            log.GetContactMethod = model.GetContactMethod;
            _logs.Commit();
            return RedirectToAction("Index");
        }
    }
}