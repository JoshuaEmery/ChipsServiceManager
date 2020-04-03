using CSMWebCore.Entities;
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
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;
        private ITicketsHistoryData _ticketsHistory;
        private IConsultationData _consultations;
        private IServicePriceData _servicePrices;
        private readonly UserManager<ChipsUser> _userManager;

        public ReportsService(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs,
            ITicketsHistoryData ticketsHistory, IConsultationData consultations,
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
        /// <summary>
        /// Gets count of active tickets, TicketStatus optional parameter
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public int TotalActiveTickets(TicketStatus? status = null)
        {

            if (!status.HasValue)
                return _tickets.GetAllActiveTickets().Count();
            else
                return _tickets.GetByStatus(status.Value).Count();
        }
        /// <summary>
        /// Gets count of completed tickets, TimeSpan optional parameter
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public int TotalCompletedTickets(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return _tickets.GetAllCompletedTickets().Count();
            else
                return _tickets.GetCompletedTickets(span.Value).Count();
        }
        /// <summary>
        /// Gets count of completed tickets between dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int TotalCompletedTickets(DateTime startDate, DateTime endDate)
        {
            return _tickets.GetCompletedTickets(startDate, endDate).Count();
        }
        /// <summary>
        /// Returns count of Checked in tickets, TimeSpan optional parameter
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public int TotalCheckedInTickets(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return _tickets.GetAll().Count();
            else
                return _tickets.GetCheckedInTickets(span.Value).Count();
        }
        /// <summary>
        /// Returns count of Checked in tickets between two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int TotalCheckedInTickets(DateTime startDate, DateTime endDate)
        {
            return _tickets.GetCheckedInTickets(startDate, endDate).Count();
        }
        /// <summary>
        /// Returns count of Checked out tickets, timespan optional parameter
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public int TotalCheckedOutTickets(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return _tickets.GetAll().Count();
            else
                return _tickets.GetCheckedOutTickets(span.Value).Count();
        }
        /// <summary>
        /// Returns count of checked out tickets between two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int TotalCheckedOutTickets(DateTime startDate, DateTime endDate)
        {
            return _tickets.GetCheckedOutTickets(startDate, endDate).Count();
        }
        /// <summary>
        /// Gets count of contact logs by a given user, timespan optional parameter
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public int GetContactLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return _logs.GetContactLogsByUser(userName).Count();
            else
                return _logs.GetContactLogsByUser(userName, span.Value).Count();
        }
        /// <summary>
        /// Gets count of contact logs by a given user between two dates
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetContactLogsByUser(string userName, DateTime startDate, DateTime endDate)
        {
            return _logs.GetContactLogsByUser(userName, startDate, endDate).Count();
        }
        /// <summary>
        /// Gets count of service logs by a given user, timespan optional parameter
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public int GetServiceLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return _logs.GetServiceLogsByUser(userName).Count();
            else
                return _logs.GetServiceLogsByUser(userName, span.Value).Count();
        }
        /// <summary>
        /// Gets count of service logs by a given user between two dates
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetServiceLogsByUser(string userName, DateTime startDate, DateTime endDate)
        {
            return _logs.GetServiceLogsByUser(userName, startDate, endDate).Count();
        }
        /// <summary>
        /// Gets count of consultations, timespan optional parameter
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public int GetConsultations(TimeSpan? span = null)
        {
            if (!span.HasValue)
                return _consultations.GetAll().Count();
            else
                return _consultations.GetConsultations(span.Value).Count();
        }
        /// <summary>
        /// Gets count of consultations between two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetConsultations(DateTime startDate, DateTime endDate)
        {
            return _consultations.GetConsultations(startDate, endDate).Count();
        }
        /// <summary>
        /// Gets count of consultations by a given user, timespan optional parameter
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public int GetConsultationsLogsByUser(string userName, TimeSpan? span = null)
        {
            if (!span.HasValue)
                return _consultations.GetConsultationsByUser(userName).Count();
            else
                return _consultations.GetConsultationsByUser(userName, span.Value).Count();
        }
        /// <summary>
        /// Gets count of consultations by user between two dates
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetConsultationsLogsByUser(string userName, DateTime startDate, DateTime endDate)
        {
            return _consultations.GetConsultationsByUser(userName, startDate, endDate).Count();
        }
        /// <summary>
        /// Generates a TicketProgressReport for the given ticketId
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        //-----------Ticket Progress Report
        public TicketProgressReport PrintProgressReport(int ticketId)
        {
            return _ticketsHistory.GetTicketProgressReport(_tickets.Get(ticketId));
        }
        /// <summary>
        /// Gets total amount of savings by ticketId
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public decimal GetSavingsByTicket(int ticketId)
        {
            return _servicePrices.GetTotalPrice(_logs.GetDistinctLogTypesByTicketId(ticketId));            
        }
        /// <summary>
        /// Gets total savings for all tickets.  Optional timespan parameter
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public decimal GetTicketSavingsOverTimePeriod(TimeSpan? span = null)
        {
            IEnumerable<Ticket> tickets;
            if (!span.HasValue)
            {
                tickets = _tickets.GetAll();
            }
            else
            {
                tickets = _tickets.GetCheckedInTickets(span.Value);
            }
            decimal total = 0m;
            foreach (var ticket in tickets)
            {
                total += _servicePrices.GetTotalPrice(_logs.GetDistinctLogTypesByTicketId(ticket.Id));
            }
            return total;
        }
        /// <summary>
        /// Gets the total savings for all tickets between two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal GetTicketSavingsOverTimePeriod(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Ticket> tickets = _tickets.GetCheckedInTickets(startDate, endDate);
            decimal total = 0m;
            foreach (var ticket in tickets)
            {
                total += _servicePrices.GetTotalPrice(_logs.GetDistinctLogTypesByTicketId(ticket.Id));
            }
            return total;
        }
        /// <summary>
        /// Gets the total savings for all consultations.  Optional timespan parameter
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public decimal GetConsultSavingsOverTimePeriod(TimeSpan? span = null)
        {
            IEnumerable<Consultation> consults;
            if (!span.HasValue)
            {
                consults = _consultations.GetAll();
            }
            else
            {
                consults = _consultations.GetConsultations(span.Value);
            }
            decimal total = 0m;
            total += _servicePrices.GetPriceOfServiceType(LogType.Diagnostic) * consults.Count();
            return total;
        }
        /// <summary>
        /// Gets the total consultation savings between two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal GetConsultSavingsOverTimePeriod(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Consultation> consults = _consultations.GetConsultations(startDate, endDate);
            decimal total = 0m;
            total += _servicePrices.GetPriceOfServiceType(LogType.Diagnostic) * consults.Count();
            return total;
        }
    }
}
