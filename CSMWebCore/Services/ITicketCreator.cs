using CSMWebCore.Models;
using CSMWebCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface ITicketCreator
    {
        TicketConfirmationModel CreateTicket(TicketCreatorInfo model);
    }
}
