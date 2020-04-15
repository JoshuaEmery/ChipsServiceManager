using CSMWebCore.Data;
using CSMWebCore.Entities;
using CSMWebCore.Enums;
using CSMWebCore.Models;
using CSMWebCore.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public class ReportsService
    {
        private ChipsDbContext context;
        private ITicketsHistoryData _ticketsHistory;

        private IServicePriceData _servicePrices;
        private readonly UserManager<ChipsUser> _userManager;

        public ReportsService(ChipsDbContext context, ITicketsHistoryData ticketsHistory,
            IServicePriceData servicePrices, UserManager<ChipsUser> userManager)
        {
            this.context = context;
            _ticketsHistory = ticketsHistory;
            _userManager = userManager;
            _servicePrices = servicePrices;
        }

        //public int TotalActiveTickets(TicketStatus? status = null)
        //{
        //    if (!status.HasValue)
        //        return context.Tickets.GetOpenTickets().Count();
        //    else
        //        return context.Tickets.GetByStatus(status.Value).Count();
        //}

        // * use context.Tickets.GetOpenTickets().Count();

        //public int TotalCompletedTickets(TimeSpan? span = null)
        //{
        //    if (!span.HasValue)
        //        return context.Tickets.GetCompletedTickets().Count();
        //    else
        //        return context.Tickets.GetCompletedTickets(span.Value).Count();
        //}

        // * use context.Tickets.GetCompletedTickets(span).Count();

        public int TotalCompletedTickets(DateTime startDate, DateTime endDate)
        {
            return context.Tickets.GetCompletedTickets(startDate, endDate).Count();
        }

        public int TotalCheckedInTickets(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return context.Tickets.Get().Count();
            else
                return context.Tickets.GetAllTicketsWithCheckIn(span.Value).Count();
        }

        public int TotalCheckedInTickets(DateTime startDate, DateTime endDate)
        {
            return context.Tickets.GetAllTicketsWithCheckIn(startDate, endDate).Count();
        }

        public int TotalCheckedOutTickets(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return context.Tickets.Get().Count();
            else
                return context.Tickets.GetClosedTickets(span.Value).Count();
        }

        public int TotalCheckedOutTickets(DateTime startDate, DateTime endDate)
        {
            return context.Tickets.GetClosedTickets(startDate, endDate).Count();
        }

        public int GetContactLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return context.Logs.GetContactLogsByUser(userName).Count();
            else
                return context.Logs.GetContactLogsByUser(userName, span.Value).Count();
        }

        public int GetContactLogsByUser(string userName, DateTime startDate, DateTime endDate)
        {
            return context.Logs.GetContactLogsByUser(userName, startDate, endDate).Count();
        }

        public int GetServiceLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return context.Logs.GetServiceLogsByUser(userName).Count();
            else
                return context.Logs.GetServiceLogsByUser(userName, span.Value).Count();
        }

        public int GetServiceLogsByUser(string userName, DateTime startDate, DateTime endDate)
        {
            return context.Logs.GetServiceLogsByUser(userName, startDate, endDate).Count();
        }

        public int GetConsultations(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return context.Consultations.Get().Count();
            else
                return context.Consultations.GetConsultations(span.Value).Count();
        }

        public int GetConsultations(DateTime startDate, DateTime endDate)
        {
            return context.Consultations.GetConsultations(startDate, endDate).Count();
        }

        public int GetConsultationsLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return context.Consultations.GetConsultationsByUser(userName).Count();
            else
                return context.Consultations.GetConsultationsByUser(userName, span.Value).Count();
        }

        public int GetConsultationsLogsByUser(string userName, DateTime startDate, DateTime endDate)
        {
            return context.Consultations.GetConsultationsByUser(userName, startDate, endDate).Count();
        }

        public TicketProgressReport PrintProgressReport(int ticketId)
        {
            return context.Tickets.Find(ticketId).GetTicketProgressReport();
        }

        //public decimal GetSavingsByTicket(int ticketId)
        //{
        //    return _servicePrices.GetTotalPrice(context.Logs.GetDistinctEventsByTicketId(ticketId));            
        //}

        // use ServicePriceData.GetTotalPriceOfTicket(ticket)

        public decimal GetTicketSavingsOverTimePeriod(TimeSpan? span = null)
        {
            IEnumerable<Ticket> tickets;
            if (!span.HasValue)
            {
                tickets = context.Tickets.Get();
            }
            else
            {
                tickets = context.Tickets.GetAllTicketsWithCheckIn(span.Value);
            }
            decimal total = 0m;
            foreach (var ticket in tickets)
            {
                total += ticket.GetTotalPriceOfTicket();
            }
            return total;
        }

        public decimal GetTicketSavingsOverTimePeriod(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Ticket> tickets = context.Tickets.GetAllTicketsWithCheckIn(startDate, endDate);
            decimal total = 0m;
            foreach (var ticket in tickets)
            {
                total += ticket.GetTotalPriceOfTicket();
            }
            return total;
        }

        public decimal GetConsultSavingsOverTimePeriod(TimeSpan? span = null)
        {
            IEnumerable<Consultation> consults;
            if (!span.HasValue)
            {
                consults = context.Consultations.Get();
            }
            else
            {
                consults = context.Consultations.GetConsultations(span.Value);
            }
            decimal total = 0m;
            total += context.Events.Find(EventEnum.Diagnostic).Price * consults.Count();
            return total;
        }

        public decimal GetConsultSavingsOverTimePeriod(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Consultation> consults = context.Consultations.GetConsultations(startDate, endDate);
            decimal total = 0m;
            total += context.Events.Find(EventEnum.Diagnostic).Price * consults.Count();
            return total;
        }

        public TimeSpan GetAverageHandleTime(TimeSpan span)
        {
            IEnumerable<Ticket> tickets = context.Tickets.GetCompletedTickets(span);
            if (tickets.Count() == 0)
            {
                return TimeSpan.Zero;
            }
            TimeSpan handleTime = new TimeSpan();
            foreach (var ticket in tickets)
            {
                handleTime += (ticket.FinishDate - ticket.CheckInDate);
            }
            return handleTime / tickets.Count();
        }
    }
}
