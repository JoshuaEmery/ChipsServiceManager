﻿using CSMWebCore.Entities;
using CSMWebCore.Enums;
using CSMWebCore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public class ReportsService : IReportsService 
    {
        private IDeviceRepository _devices;
        private ICustomerRepository _customers;
        private ITicketRepository _tickets;
        private ILogRepository _logs;
        private ITicketsHistoryData _ticketsHistory;
        private IConsultationRepository _consultations;
        private IServicePriceData _servicePrices;
        private readonly UserManager<ChipsUser> _userManager;

        public ReportsService(IDeviceRepository devices, ICustomerRepository customers, ITicketRepository tickets, ILogRepository logs,
            ITicketsHistoryData ticketsHistory, IConsultationRepository consultations,
            IServicePriceData servicePrices, UserManager<ChipsUser> userManager)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
            _ticketsHistory = ticketsHistory;
            _consultations = consultations;
            _userManager = userManager;
            _servicePrices = servicePrices;
        }

        public int TotalActiveTickets(TicketStatus? status = null)
        {

            if (!status.HasValue)
                return _tickets.GetOpen().Count();
            else
                return _tickets.GetByStatus(status.Value).Count();
        }

        public int TotalCompletedTickets(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return _tickets.GetCompleted().Count();
            else
                return _tickets.GetCompleted(span.Value).Count();
        }

        public int TotalCompletedTickets(DateTime startDate, DateTime endDate)
        {
            return _tickets.GetCompleted(startDate, endDate).Count();
        }

        public int TotalCheckedInTickets(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return _tickets.Get().Count();
            else
                return _tickets.GetAll(span.Value).Count();
        }

        public int TotalCheckedInTickets(DateTime startDate, DateTime endDate)
        {
            return _tickets.GetAll(startDate, endDate).Count();
        }

        public int TotalCheckedOutTickets(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return _tickets.Get().Count();
            else
                return _tickets.GetClosed(span.Value).Count();
        }

        public int TotalCheckedOutTickets(DateTime startDate, DateTime endDate)
        {
            return _tickets.GetClosed(startDate, endDate).Count();
        }

        public int GetContactLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return _logs.GetContactLogsByUser(userName).Count();
            else
                return _logs.GetContactLogsByUser(userName, span.Value).Count();
        }

        public int GetContactLogsByUser(string userName, DateTime startDate, DateTime endDate)
        {
            return _logs.GetContactLogsByUser(userName, startDate, endDate).Count();
        }

        public int GetServiceLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return _logs.GetServiceLogsByUser(userName).Count();
            else
                return _logs.GetServiceLogsByUser(userName, span.Value).Count();
        }

        public int GetServiceLogsByUser(string userName, DateTime startDate, DateTime endDate)
        {
            return _logs.GetServiceLogsByUser(userName, startDate, endDate).Count();
        }

        public int GetConsultations(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return _consultations.Get().Count();
            else
                return _consultations.GetConsultations(span.Value).Count();
        }

        public int GetConsultations(DateTime startDate, DateTime endDate)
        {
            return _consultations.GetConsultations(startDate, endDate).Count();
        }

        public int GetConsultationsLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return _consultations.GetConsultationsByUser(userName).Count();
            else
                return _consultations.GetConsultationsByUser(userName, span.Value).Count();
        }

        public int GetConsultationsLogsByUser(string userName, DateTime startDate, DateTime endDate)
        {
            return _consultations.GetConsultationsByUser(userName, startDate, endDate).Count();
        }

        public TicketProgressReport PrintProgressReport(int ticketId)
        {
            return _ticketsHistory.GetTicketProgressReport(_tickets.GetById(ticketId));
        }

        public decimal GetSavingsByTicket(int ticketId)
        {
            return _servicePrices.GetTotalPrice(_logs.GetDistinctLogTypesByTicketId(ticketId));            
        }

        public decimal GetTicketSavingsOverTimePeriod(TimeSpan? span = null)
        {
            IEnumerable<Ticket> tickets;
            if (!span.HasValue)
            {
                tickets = _tickets.Get();
            }
            else
            {
                tickets = _tickets.GetAll(span.Value);
            }
            decimal total = 0m;
            foreach (var ticket in tickets)
            {
                total += _servicePrices.GetTotalPrice(_logs.GetDistinctLogTypesByTicketId(ticket.Id));
            }
            return total;
        }

        public decimal GetTicketSavingsOverTimePeriod(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Ticket> tickets = _tickets.GetAll(startDate, endDate);
            decimal total = 0m;
            foreach (var ticket in tickets)
            {
                total += _servicePrices.GetTotalPrice(_logs.GetDistinctLogTypesByTicketId(ticket.Id));
            }
            return total;
        }

        public decimal GetConsultSavingsOverTimePeriod(TimeSpan? span = null)
        {
            IEnumerable<Consultation> consults;
            if (!span.HasValue)
            {
                consults = _consultations.Get();
            }
            else
            {
                consults = _consultations.GetConsultations(span.Value);
            }
            decimal total = 0m;
            total += _servicePrices.GetPriceOfServiceType(LogType.Diagnostic) * consults.Count();
            return total;
        }

        public decimal GetConsultSavingsOverTimePeriod(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Consultation> consults = _consultations.GetConsultations(startDate, endDate);
            decimal total = 0m;
            total += _servicePrices.GetPriceOfServiceType(LogType.Diagnostic) * consults.Count();
            return total;
        }

        public TimeSpan GetAverageHandleTime(TimeSpan span)
        {
            IEnumerable<Ticket> tickets = _tickets.GetCompleted(span);
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
