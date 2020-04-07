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
    //The only method in this controller that is used is the Index method
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
        //Home/Index
        //This method gathers all of the data needed for the Google Charts.  
        public IActionResult Index()
        {
            //get all active tickets
            var activeTickets = _tickets.GetOpen();
            //get all tickets
            var allTickets = _tickets.GetAll();
            //create new ViewModel
            var model = new HomeIndexViewModel();
            //see all properties to 0
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
            //Iterate through all activeTickets
            foreach (var ticket in activeTickets)
            {
                //count by status
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
                //caculate age of ticket
                TimeSpan daysOld = DateTime.Now - ticket.CheckInDate;
                //sum the age of the tickets, used for avg calc
                sumAge += daysOld;
                //check if this ticket is the oldest ticket
                if (daysOld > maxAge)
                {
                    maxAge = daysOld;
                    maxAgeTicketId = ticket.Id;
                }
                //check how long it has been since the ticket was worked on by a tech
                TimeSpan daysIdle = DateTime.Now - _logs.GetLastByTicketId(ticket.Id).Logged;
                //sum for the average
                sumIdle += daysIdle;
                if (daysIdle > maxIdle)
                {
                    maxIdle = daysIdle;
                    maxIdleTicketId = ticket.Id;
                }
            }
            //caculate the average time to finish for each ticket that has been completed
            //thining about this more, this should not be all tickets, it should be all completed
            //tickets, also it could be all completed tickets within the last year, as charts
            //does not track any data longer than a year

            //count how many tickets have been finished in the last week, month, 90 days, and year
            //as well as the average handle time over that period
            foreach (var ticket in allTickets)
            {
                if((DateTime.Now - ticket.FinishDate).TotalDays < 8)
                {
                    weekCount++;
                    weekTotal += ticket.FinishDate - ticket.CheckInDate;
                    monthCount++;
                    monthTotal += ticket.FinishDate - ticket.CheckInDate;
                    ninetyCount++;
                    ninetyTotal += ticket.FinishDate - ticket.CheckInDate;
                    yearCount++;
                    yearTotal += ticket.FinishDate - ticket.CheckInDate;
                }
                else if ((DateTime.Now - ticket.FinishDate).TotalDays < 31)
                {
                    monthCount++;
                    monthTotal += ticket.FinishDate - ticket.CheckInDate;
                    ninetyCount++;
                    ninetyTotal += ticket.FinishDate - ticket.CheckInDate;
                    yearCount++;
                    yearTotal += ticket.FinishDate - ticket.CheckInDate;
                }
                else if ((DateTime.Now - ticket.FinishDate).TotalDays < 91)
                {
                    ninetyCount++;
                    ninetyTotal += ticket.FinishDate - ticket.CheckInDate;
                    yearCount++;
                    yearTotal += ticket.FinishDate - ticket.CheckInDate;
                }
                else if ((DateTime.Now - ticket.FinishDate).TotalDays < 366)
                {
                    yearCount++;
                    yearTotal += ticket.FinishDate - ticket.CheckInDate;
                }

            }
            if(activeTickets.Count() > 0)
            {
                TimeSpan avgAge = sumAge / activeTickets.Count();
                TimeSpan avgIdle = sumIdle / activeTickets.Count();
                model.avgAge = avgAge;
                model.avgIdle = avgIdle;
            }
            //update model
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
        //Home/Charts
        //This Method and the corresponding view was used for testing the Google Charts API.
        //I have left it here to try out other google Charts if needed.  This is not needed by the application
        public IActionResult Charts()
        {
            var activeTickets = _tickets.GetOpen();
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
                TimeSpan daysOld = DateTime.Now - ticket.CheckInDate;
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
        //I dont remember what this came from, pretty sure it was auto generated and I dont know what it does.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        


    }
}
