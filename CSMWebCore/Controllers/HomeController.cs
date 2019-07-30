using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSMWebCore.Models;
using Microsoft.AspNetCore.Authorization;
using CSMWebCore.Services;
using CSMWebCore.ViewModels;

namespace CSMWebCore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;
        
        public HomeController(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
            
        }
        public IActionResult Index()
        {
            var activeTickets = _tickets.GetAllActiveTickets();
            var model = new HomeIndexViewModel();
            model.newCount = 0;
            model.inProgressCount = 0;
            model.pendingResponseCount = 0;
            model.pendingPickupCount = 0;
            TimeSpan maxAge = TimeSpan.Zero;
            TimeSpan sumAge = TimeSpan.Zero;
            int maxAgeTicketId = 0;
            TimeSpan maxIdle = TimeSpan.Zero;
            TimeSpan sumIdle = TimeSpan.Zero;
            int maxIdleTicketId = 0;
            foreach (var ticket in activeTickets)
            {
                switch (ticket.TicketStatus)
                {
                    case TicketStatus.New:
                        model.newCount++;
                        break;
                    case TicketStatus.InProgress:
                        model.inProgressCount++;
                        break;
                    case TicketStatus.PendingResponse:
                        model.pendingResponseCount++;
                        break;
                    case TicketStatus.PendingPickup:
                        model.pendingPickupCount++;
                        break;
                }
                TimeSpan daysOld = DateTime.Now - ticket.CheckedIn;
                sumAge += daysOld;
                if (daysOld > maxAge)
                {
                    maxAge = daysOld;
                    maxAgeTicketId = ticket.Id;
                }
                TimeSpan daysIdle = DateTime.Now - _logs.GetLastByTicketId(ticket.Id).Logged;
                sumIdle += daysIdle;
                if(daysIdle > maxIdle)
                {
                    maxIdle = daysIdle;
                    maxIdleTicketId = ticket.Id;
                }
            }
            TimeSpan avgAge = sumAge / activeTickets.Count();
            TimeSpan avgIdle = sumIdle / activeTickets.Count();
            model.avgAge = avgAge;
            model.avgIdle = avgIdle;
            model.maxAge = maxAge;
            model.maxAgeTicketId = maxAgeTicketId;
            model.maxIdle = maxIdle;
            model.maxIdleTicketId = maxIdleTicketId;



            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
