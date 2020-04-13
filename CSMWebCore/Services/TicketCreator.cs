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
        private IDeviceRepository _devices;
        private ICustomerRepository _customers;
        private ITicketRepository _tickets;
        private ILogRepository _logs;
        private IUpdateData _updates;
        public TicketCreator(IDeviceRepository devices, ICustomerRepository customers, ITicketRepository tickets, ILogRepository logs, IUpdateData updates)
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
                Status = TicketStatus.New,
                TicketNumber = _tickets.GetLatestTicketNum() + 1
            };
            //Save the new Ticket
            _tickets.Insert(ticket);
            _tickets.Commit();
            // TODO don't commit ticket until initial log has been created -- if log creation fails an incomplete ticket is created
            //Create a log entry with the newly created Ticket.Id as a foreign key
            Log log = new Log
            {
                EventId = (int)EventName.CheckIn,
                // TODO get event id dynamically from db
                UserCreated = info.UserName,
                TicketId = ticket.Id,
                DateCreated = DateTime.Now,
                Notes = info.Notes,
                LogType = LogType.CheckIn,
                ContactMethod = ContactMethod.InPerson
            };
            //Add new Log
            _logs.Insert(log);
            _logs.Commit();
            //Create a new entry in the update table with a guid for the Primary Key and
            //a foreign key from the Ticket
            TicketProgress update = new TicketProgress
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
