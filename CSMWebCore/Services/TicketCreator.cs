using CSMWebCore.Data;
using CSMWebCore.Entities;
using CSMWebCore.Enums;
using CSMWebCore.Models;
using CSMWebCore.Shared;
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
        private ChipsDbContext context;
        private IUpdateData _updates;
        public TicketCreator(ChipsDbContext context, IUpdateData updates)
        {
            this.context = context;
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
                TicketNumber = context.Tickets.GetLatestTicketNum() + 1
            };
            //Save the new Ticket
            context.Add(ticket);
            context.SaveChanges();
            // TODO don't commit ticket until initial log has been created -- if log creation fails an incomplete ticket is created
            //Create a log entry with the newly created Ticket.Id as a foreign key
            Log log = new Log
            {
                EventId = (int)EventEnum.CheckIn,
                // TODO get event id dynamically from db
                UserCreated = info.UserName,
                TicketId = ticket.Id,
                DateCreated = DateTime.Now,
                Notes = info.Notes
            };
            //Add new Log
            context.Add(log);
            context.SaveChanges();
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
