using CSMWebCore.Entities;
using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Event GetCheckInEvent();
        Event GetCheckOutEvent();
    }
}
