using CSMWebCore.Data;
using CSMWebCore.Entities;
using CSMWebCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(ChipsDbContext db) : base(db)
        { }

        // refers to IDs assigned in seed data in Data/ChipsDbContext
        public Event GetCheckInEvent() => GetById(1);
        public Event GetCheckOutEvent() => GetById(2);
        public IEnumerable<Event> GetEventsByCategory(EventCategory category) => Get(filter: e => e.Category == category);
    }
}
