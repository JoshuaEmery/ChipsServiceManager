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
using CSMWebCore.Entities;

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
            var allTickets = _tickets.GetAll();
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
            TimeSpan weekTotal = TimeSpan.Zero;
            int weekCount = 0;
            TimeSpan weekAvgHandle = TimeSpan.Zero;
            TimeSpan monthTotal = TimeSpan.Zero;
            int monthCount = 0;
            TimeSpan monthAvgHandle = TimeSpan.Zero;
            TimeSpan ninetyTotal = TimeSpan.Zero;
            int ninetyCount = 0;
            TimeSpan ninetyAvgHandle = TimeSpan.Zero;
            TimeSpan yearTotal = TimeSpan.Zero;
            int yearCount = 0;
            TimeSpan yearAvgHandle = TimeSpan.Zero;            
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
                if (daysIdle > maxIdle)
                {
                    maxIdle = daysIdle;
                    maxIdleTicketId = ticket.Id;
                }
            }
            foreach (var ticket in allTickets)
            {
                if((DateTime.Now - ticket.Finished).TotalDays < 8)
                {
                    weekCount++;
                    weekTotal += ticket.Finished - ticket.CheckedIn;
                    monthCount++;
                    monthTotal += ticket.Finished - ticket.CheckedIn;
                    ninetyCount++;
                    ninetyTotal += ticket.Finished - ticket.CheckedIn;
                    yearCount++;
                    yearTotal += ticket.Finished - ticket.CheckedIn;
                }
                else if ((DateTime.Now - ticket.Finished).TotalDays < 31)
                {
                    monthCount++;
                    monthTotal += ticket.Finished - ticket.CheckedIn;
                    ninetyCount++;
                    ninetyTotal += ticket.Finished - ticket.CheckedIn;
                    yearCount++;
                    yearTotal += ticket.Finished - ticket.CheckedIn;
                }
                else if ((DateTime.Now - ticket.Finished).TotalDays < 91)
                {
                    ninetyCount++;
                    ninetyTotal += ticket.Finished - ticket.CheckedIn;
                    yearCount++;
                    yearTotal += ticket.Finished - ticket.CheckedIn;
                }
                else if ((DateTime.Now - ticket.Finished).TotalDays < 366)
                {
                    yearCount++;
                    yearTotal += ticket.Finished - ticket.CheckedIn;
                }

            }
            if(activeTickets.Count() > 0)
            {
                TimeSpan avgAge = sumAge / activeTickets.Count();
                TimeSpan avgIdle = sumIdle / activeTickets.Count();
                model.avgAge = avgAge;
                model.avgIdle = avgIdle;
            }
            model.maxAge = maxAge;
            model.maxAgeTicketId = maxAgeTicketId;
            model.maxIdle = maxIdle;
            model.maxIdleTicketId = maxIdleTicketId;
            model.weekAvgHandle = weekCount == 0 ? TimeSpan.Zero : weekTotal / weekCount;
            model.monthAvgHandle = monthCount == 0 ? TimeSpan.Zero : monthTotal / monthCount;
            model.ninetyDayAvgHandle = ninetyCount == 0 ? TimeSpan.Zero : ninetyTotal / ninetyCount;
            model.yearAvgHangle = yearCount == 0 ? TimeSpan.Zero : yearTotal / yearCount;



            return View(model);
        }
        public IActionResult Charts()
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
                if (daysIdle > maxIdle)
                {
                    maxIdle = daysIdle;
                    maxIdleTicketId = ticket.Id;
                }
            }
            if(activeTickets.Count() > 0)
            {
                TimeSpan avgAge = sumAge / activeTickets.Count();
                TimeSpan avgIdle = sumIdle / activeTickets.Count();
                model.avgAge = avgAge;
                model.avgIdle = avgIdle;
            }

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
