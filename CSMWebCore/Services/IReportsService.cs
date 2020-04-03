using CSMWebCore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface IReportsService
    {
        int TotalActiveTickets(TicketStatus? status = null);
        int TotalCompletedTickets(TimeSpan? span = null);
        int TotalCompletedTickets(DateTime startDate, DateTime endDate);
        int TotalCheckedInTickets(TimeSpan? span = null);
        int TotalCheckedInTickets(DateTime startDate, DateTime endDate);
        int TotalCheckedOutTickets(TimeSpan? span = null);
        int TotalCheckedOutTickets(DateTime startDate, DateTime endDate);
        int GetContactLogsByUser(string userName, TimeSpan? span = null);
        int GetContactLogsByUser(string userName, DateTime startDate, DateTime endDate);
        int GetServiceLogsByUser(string userName, TimeSpan? span = null);
        int GetServiceLogsByUser(string userName, DateTime startDate, DateTime endDate);
        int GetConsultations(TimeSpan? span = null);
        int GetConsultations(DateTime startDate, DateTime endDate);
        int GetConsultationsLogsByUser(string userName, TimeSpan? span = null);
        int GetConsultationsLogsByUser(string userName, DateTime startDate, DateTime endDate);
        TicketProgressReport PrintProgressReport(int ticketId);
        decimal GetSavingsByTicket(int ticketId);
        decimal GetTicketSavingsOverTimePeriod(TimeSpan? span = null);
        decimal GetTicketSavingsOverTimePeriod(DateTime startDate, DateTime endDate);
        decimal GetConsultSavingsOverTimePeriod(TimeSpan? span = null);
        decimal GetConsultSavingsOverTimePeriod(DateTime startDate, DateTime endDate);
    }
}
