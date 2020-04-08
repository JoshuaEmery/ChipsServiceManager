using CSMWebCore.Entities;
using CSMWebCore.Models;
using CSMWebCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public class TicketCreator : ITicketCreator
    {
        private IDeviceData _devices;
        private ICustomerData _customers;
        private ITicketData _tickets;
        private ILogData _logs;
        private IUpdateData _updates;
        public TicketCreator(IDeviceData devices, ICustomerData customers, ITicketData tickets, ILogData logs, IUpdateData updates)
        {
            _devices = devices;
            _customers = customers;
            _tickets = tickets;
            _logs = logs;
            _updates = updates;
        }

        public TicketConfirmationModel CreateTicket(TicketCreatorInfo info)
        {
            Ticket ticket = new Ticket
            {
                DeviceId = info.DeviceId,
                //Gets a string representation of the ChipsUser currently logged in
                CheckInUserId = info.UserName,
                CheckInDate = DateTime.Now,
                NeedsBackup = info.NeedsBackup,
                TicketStatus = TicketStatus.New,
                TicketNumber = _tickets.GetLatestTicketNum() + 1
            };
            //Save the new Ticket
            _tickets.Add(ticket);
            _tickets.Commit();
            //Create a log entry with the newly created Ticket.Id as a foreign key
            Log log = new Log
            {
                UserId = info.UserName,
                TicketId = ticket.Id,
                Logged = DateTime.Now,
                Notes = info.Notes,
                LogType = LogType.CheckIn,
                ContactMethod = ContactMethod.InPerson
            };
            //Add new Log
            _logs.Add(log);
            _logs.Commit();
            //Create a new entry in the update table with a guid for the Primary Key and
            //a foreign key from the Ticket
            Update update = new Update
            {
                Id = new Guid(),
                TicketId = ticket.Id
            };
            //Save Changes
            _updates.Add(update);
            _updates.Commit();
            return new TicketConfirmationModel
            {
                ticketId = ticket.Id,
                deviceId = info.DeviceId,
                customerId = info.CustomerId,
                updateId = update.Id
            };
        }
    }
}
